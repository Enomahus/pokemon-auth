﻿using AnthentificationApi_Jwt.Constants;
using AnthentificationApi_Jwt.Contexts;
using AnthentificationApi_Jwt.Entities;
using AnthentificationApi_Jwt.Models;
using AnthentificationApi_Jwt.Settings;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AnthentificationApi_Jwt.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly JWT _jwt;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IOptions<JWT> jwt)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _jwt = jwt.Value;
    }  

    /// <summary>
    /// Permet de créer un nouvel utilisateur tout en vérifiant s'il n'exist pas déjà en base puis l'ajout d'un rôle
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
        var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
        if (userWithSameEmail == null)
        {
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Authorization.default_role.ToString());

            }
            return $"User Registered with username {user.UserName}";
        }
        else
        {
            return $"Email {user.Email} is already registered.";
        }
    }

    public async Task<AuthenticationModel> Login(LoginModel model)
    {
        var authenticationModel = new AuthenticationModel();
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) 
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
            return authenticationModel;
        }
        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authenticationModel.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            //authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Token = jwtSecurityToken.EncodedPayload;
            authenticationModel.Email = user.Email;
            authenticationModel.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList();

            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                authenticationModel.RefreshToken = activeRefreshToken.Token;
                authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                authenticationModel.RefreshToken = refreshToken.Token;
                authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _context.Update(user);
                _context.SaveChanges();
            }

            return authenticationModel;
        }

        authenticationModel.IsAuthenticated = false;
        authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
        return authenticationModel;
    }

    /// <summary>
    /// Permet la création de Token Jwt en rameant les infos utilisateur et le Token à partir du model de reponse {AuthenticationModel}
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
    {
        var authenticationModel = new AuthenticationModel();
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"No Accounts Registered with {model.Email}.";
            return authenticationModel;
        }

        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authenticationModel.IsAuthenticated = true;
            JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
            //authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.Token = jwtSecurityToken.EncodedPayload;
            authenticationModel.Email = user.Email;
            authenticationModel.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenticationModel.Roles = rolesList.ToList();

            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                authenticationModel.RefreshToken = activeRefreshToken.Token;
                authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                authenticationModel.RefreshToken = refreshToken.Token;
                authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _context.Update(user);
                _context.SaveChanges();
            }

            return authenticationModel;
        }

        authenticationModel.IsAuthenticated = false;
        authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
        return authenticationModel;
    }

    /// <summary>
    /// Permet l'actualisation du token suite à expiration
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<AuthenticationModel> RefreshTokenAsync(string token)
    {
        var authenticationModel = new AuthenticationModel();
        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
        if (user == null)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Token did not match any users.";
            return authenticationModel;
        }

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        if (!refreshToken.IsActive)
        {
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Token Not Active.";
            return authenticationModel;
        }

        //Revoke Current Refresh Token
        refreshToken.Revoked = DateTime.UtcNow;

        //Generate new Refresh Token and save to Database
        var newRefreshToken = CreateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        _context.Update(user);
        _context.SaveChanges();

        //Generates new jwt
        authenticationModel.IsAuthenticated = true;
        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
        //authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authenticationModel.Token = jwtSecurityToken.EncodedPayload;
        authenticationModel.Email = user.Email;
        authenticationModel.UserName = user.UserName;
        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        authenticationModel.Roles = rolesList.ToList();
        authenticationModel.RefreshToken = newRefreshToken.Token;
        authenticationModel.RefreshTokenExpiration = newRefreshToken.Expires;
        return authenticationModel;
    }

    public ApplicationUser GetById(string id)
    {
        return _context.Users.Find(id);
    }

   

    public async Task<bool> RevokeToken(string token)
    {
        var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

        // return false if no user found with token
        if (user == null) return false;

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

        // return false if token is not active
        if (!refreshToken.IsActive) return false;

        // revoke token and save
        refreshToken.Revoked = DateTime.UtcNow;
        _context.Update(user);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Permet l'ajout d'un Role
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<string> AddRoleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return $"No Accounts Registered with {model.Email}.";
        }
        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var roleExists = Enum.GetNames(typeof(Authorization.Roles)).Any(x => x.ToLower() == model.Role.ToLower());
            if (roleExists)
            {
                var validRole = Enum.GetValues(typeof(Authorization.Roles)).Cast<Authorization.Roles>().Where(x => x.ToString().ToLower() == model.Role.ToLower()).FirstOrDefault();
                await _userManager.AddToRoleAsync(user, validRole.ToString());
                return $"Added {model.Role} to user {model.Email}.";
            }
            return $"Role {model.Role} not found.";
        }
        return $"Incorrect Credentials for user {user.Email}.";
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);       

        return jwtSecurityToken;
    }
    

    /// <summary>
    /// Permet de créer et renvoyer un ibjet TokenRefresh avec des propriétés telles que Token, Expiration Data et Created Data
    /// </summary>
    /// <returns></returns>
    private RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var generator = new RNGCryptoServiceProvider())
        {
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(10),
                Created = DateTime.UtcNow
            };

        }
    }

    
}

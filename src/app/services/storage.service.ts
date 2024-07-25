import { Injectable } from '@angular/core';
import { IUserModel } from '../Models/userModel';

const USER_KEY = "authenticated-user";

@Injectable({
  providedIn: 'root'
})

/**
 * StorageService gère le stockage, la récupération et la supression des informations de l'utilisateur authentifié (User) dans le localStorage du navigateur. Cela permet de maintenir la persistance des données de l'utilisateur entre les sessions de navigation.
 */
export class StorageService {

  saveUser(user: IUserModel){
    window.localStorage.removeItem(USER_KEY);
    window.localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  getSavedUser() : IUserModel | null {
    const user = window.localStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }
    return null;
  }

  clean(): void {
    window.localStorage.clear();
  }
}

import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common'
import { Router, RouterOutlet, RouterLink } from '@angular/router';
import { Pokemon } from '../pokemon';
import { PokemonService } from '../pokemon.service';
import { PokemonSearchComponent } from "../pokemon-search/pokemon-search.component";
import { BorderCardDirective } from '../border-card-directive';
import { PokemonTypeColorPipe } from '../pokemon-type-color.pipe';

@Component({
  selector: 'app-pokemon-list',
  standalone: true,
  imports: [CommonModule, RouterOutlet,RouterLink, PokemonTypeColorPipe, PokemonSearchComponent, BorderCardDirective],
  templateUrl: './pokemon-list.component.html',
  styleUrl: './pokemon-list.component.css'
})
export class PokemonListComponent implements OnInit {
  pokemonList!: Pokemon[];
  pokemonService = inject(PokemonService);
  router = inject(Router);

  ngOnInit(){
    this.pokemonService.getPokemonList()
      .subscribe(data => { 
        debugger;
        console.log(data);
        this.pokemonList = data;
      });
  }

  goToPokemon(pokemon: Pokemon){
    debugger;
    this.router.navigate(['/pokemons/pokemon-detail', pokemon.id]);
  }

 
}

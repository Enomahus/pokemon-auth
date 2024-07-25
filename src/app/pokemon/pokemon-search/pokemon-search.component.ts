import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Pokemon } from '../pokemon';
import { Observable, Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { Router } from '@angular/router';
import { PokemonService } from '../pokemon.service';

@Component({
  selector: 'app-pokemon-search',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pokemon-search.component.html',
  styleUrl: './pokemon-search.component.css'
})
export class PokemonSearchComponent implements OnInit{
  searchTerms = new Subject<string>();
  pokemons$!: Observable<Pokemon[]>;

  constructor(
    private router: Router,
    private pokemonService: PokemonService
    ){}

    ngOnInit(): void {
      this.pokemons$ = this.searchTerms.pipe(
        // {..."a"."ab"..."abz"."ab"...."abc"......}
        debounceTime(300),
        // {....."ab"...."ab"...."abc"......}
        distinctUntilChanged(),
        // {......"ab"........"abc"......}
        switchMap((term) => this.pokemonService.searchPokemonList(term))
      );
    }
  
    search(term: string){
      this.searchTerms.next(term);
    }
  
    goToDetail(pokemon: Pokemon){
      const link = ['/pokemons/pokemon-detail',pokemon.id];
      this.router.navigate(link);
    }
}

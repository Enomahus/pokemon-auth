import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, of, tap } from 'rxjs';
import { Pokemon } from './pokemon';

@Injectable({
  providedIn: 'root'
})
export class PokemonService {

  http = inject(HttpClient);

  getPokemonList(): Observable<Pokemon[]> {
    return this.http.get<Pokemon[]>('api/pokemons').pipe(
      tap((response) => this.log(response)),
      catchError((error) =>this.handleError(error,[]))
    );
  }

  getPokemonById(pokemonId: number): Observable<Pokemon | undefined> {
    const headers ={ 'Content-Type': 'application/json'};
    return this.http.get<Pokemon>(`api/pokemons/${pokemonId}`, {'headers':headers})
    .pipe(
      tap((response) => this.log(response)),
      catchError((error) =>this.handleError(error,undefined))
    );
  }

  updatePokemon(pokemon:Pokemon): Observable<null>{
      const headers ={ 'Content-Type': 'application/json'};
      const body = JSON.stringify(pokemon);
    return this.http.put(`api/pokemons/${pokemon.id}`, body, {'headers':headers})
    .pipe(
      tap((response) => this.log(response)),
      catchError((error) => this.handleError(error, null))
    );
  }

  //addPokemon(pokemon: Pokemon): Observable<Pokemon>{
  addPokemon(pokemon: Pokemon): Observable<Pokemon>{
    const headers = { 'Content-Type': 'application/json'}
    const body = JSON.stringify(pokemon);
    return this.http.post<Pokemon>('api/pokemons', body, {'headers':headers})
    .pipe(
       tap((response) => this.log(response)),
       catchError((error) => this.handleError(error, null))
    );
  }

  deletePokemonById(pokemonId: number): Observable<null>{
    console.log(pokemonId);
    return this.http.delete(`api/pokemons/${pokemonId}`).pipe(
      tap((response) => this.log(response)),
      catchError((error) => this.handleError(error, null))
    );
  }

  searchPokemonList(term: string): Observable<Pokemon[]>{
    if(term.length <= 1){
      return of([]);
    }
    return this.http.get<Pokemon[]>(`api/pokemons/?name=${term}`)
    .pipe(
      tap((response) => this.log(response)),
      catchError((error) => this.handleError(error, []))
    );
  } 

  private log(response: any){
    console.table(response);
  }
  
  private handleError(error: Error, errorValue: any){
    console.error(error);
    return of(errorValue);
  }

  getPokemonTypeList(): string[] {
    return ['Plante','Feu','Eau','Insecte','Normal','Electrik','Poison','Fée','Vol','Combat','Psy'];
  }
}

import { Component, Input, OnInit, inject } from '@angular/core';
import { PokemonService } from '../pokemon.service';
import { Router } from '@angular/router';
import { Pokemon } from '../pokemon';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LoaderComponent } from '../loader/loader.component';
import { PokemonTypeColorPipe } from '../pokemon-type-color.pipe';

@Component({
  selector: 'app-pokemon-form',
  standalone: true,
  imports: [CommonModule, FormsModule, PokemonTypeColorPipe, LoaderComponent],
  templateUrl: './pokemon-form.component.html',
  styleUrl: './pokemon-form.component.css'
})
export class PokemonFormComponent implements OnInit{
  @Input() pokemon!: Pokemon;
  types: string[] = [];
  isAddForm: boolean = false;

  pokemonService = inject(PokemonService);
  router = inject(Router)

    ngOnInit() {
      this.types = this.pokemonService.getPokemonTypeList();
      this.isAddForm = this.router.url.includes('add'); // Permet de lire l'url pour afin de savoir le type d'opération (Ajout/Modification) à faire
    }

    // Verifier si un pokemon a ou pas un type
  hasType(type: string): boolean {
    return this.pokemon.types.includes(type);
  }

  // Permettre de cocher ou décocher les case de Type
  selectType($event: Event, type: string){
    const isChecked: boolean = ($event.target as HTMLInputElement).checked;

    if(isChecked){
      this.pokemon.types.push(type);
    }else{
      const index = this.pokemon.types.indexOf(type);
      this.pokemon.types.splice(index, 1);
    }
  }

  // Permet de bloquer/débloquer les checbox en fonction des règles de validation determinées [règle de validation personnalisée]
  isTypesValid(type: string): boolean {

    if(this.pokemon.types.length == 1 && this.hasType(type)){
      return false;
    }

    if(this.pokemon.types.length > 2 && !this.hasType(type)){
      return false;
    }

    return true;
  }

  onSubmit() {
    debugger;
    if(this.isAddForm){
      this.pokemonService.addPokemon(this.pokemon)
      .subscribe(data => {
        debugger;
        console.log(data);
        this.router.navigate(['/pokemons/pokemon-detail', data.id]);
      });
    }
    else{
      this.pokemonService.updatePokemon(this.pokemon)
      .subscribe(() => {
        this.router.navigate(['/pokemons/pokemon-detail',this.pokemon.id]); 
      });
    }
  }
}

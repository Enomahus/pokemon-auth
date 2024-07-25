import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { PokemonTypeColorPipe } from '../pokemon-type-color.pipe';
import { LoaderComponent } from '../loader/loader.component';
import { Pokemon } from '../pokemon';
import { ActivatedRoute, Router } from '@angular/router';
import { PokemonService } from '../pokemon.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-pokemon-detail',
  standalone: true,
  imports: [CommonModule,PokemonTypeColorPipe, LoaderComponent],
  templateUrl: './pokemon-detail.component.html',
  styleUrl: './pokemon-detail.component.css'
})
export class PokemonDetailComponent implements OnInit{
  pokemonList!: Pokemon[];
  pokemon: Pokemon | undefined;

  route = inject(ActivatedRoute);
  router = inject(Router);
  pokemonService = inject(PokemonService);
  title = inject(Title);

  // constructor(private route: ActivatedRoute, 
  //   private router: Router, 
  //   private pokemonService: PokemonService,
  //   private title: Title
  //   ) {}

  ngOnInit()  
  {
    const pokemonId: string|null = this.route.snapshot.paramMap.get('id');
    console.log(pokemonId);
    if(pokemonId){
      debugger;
      this.pokemonService.getPokemonById(+pokemonId)
        .subscribe(pokemon => {
          this.pokemon = pokemon;
          this.initTitle(pokemon);
        });
    }
  }

  deletePokemon(pokemon: Pokemon){
    this.pokemonService.deletePokemonById(pokemon.id)
      .subscribe(() => this.goToPokemonList());
  }

  goToPokemonList(){
    this.router.navigate(['/pokemons']);
  }

  goToEditPokemon(pokemon: Pokemon){
    debugger;
    console.log(pokemon.id);
    this.router.navigate(['/pokemons/pokemon-edit/',pokemon.id]);
  }

  initTitle(pokemon: Pokemon|undefined){
    if(!pokemon){
      this.title.setTitle('Pokemon not found');
      return;
    }
    this.title.setTitle(pokemon.name);
  }
}

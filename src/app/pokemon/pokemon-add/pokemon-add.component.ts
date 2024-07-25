import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { PokemonFormComponent } from '../pokemon-form/pokemon-form.component';
import { Pokemon } from '../pokemon';

@Component({
  selector: 'app-pokemon-add',
  standalone: true,
  imports: [CommonModule, PokemonFormComponent],
  templateUrl: './pokemon-add.component.html',
  styleUrl: './pokemon-add.component.css'
})
export class PokemonAddComponent implements OnInit {
  pokemon!: Pokemon;

  ngOnInit() {
    this.pokemon = new Pokemon();
  }
}

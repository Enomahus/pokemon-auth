import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./pokemon.component').then(c => c.PokemonComponent),
        children: [
            {
                path: '',
                title: 'Pokemons list',
                loadComponent: () => import('./pokemon-list/pokemon-list.component').then(c => c.PokemonListComponent)
            },
            {
                path: 'pokemon-add',
                title: 'Add pokemon',
                loadComponent: () => import('./pokemon-add/pokemon-add.component').then(c => c.PokemonAddComponent)
            },
            {
                path: 'pokemon-detail/:id',
                loadComponent: () => import('./pokemon-detail/pokemon-detail.component').then(c => c.PokemonDetailComponent)
            },
            {
                path: 'pokemon-edit/:id',
                loadComponent: () => import('./pokemon-edit/pokemon-edit.component').then(c => c.PokemonEditComponent)
            }
        ]
    }
];

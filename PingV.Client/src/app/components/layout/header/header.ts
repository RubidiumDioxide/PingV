import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';


@Component({
    selector: 'app-header',
    imports: [ 
        MatIconModule, 
        MatToolbarModule,  
    ],
    templateUrl: './header.html',
    styleUrl: './header.scss',
})
export class Header {}

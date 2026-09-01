import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon'; 
import { CascadingService } from '../../../service/cascading.service';


@Component({
    selector: 'app-user-select',
    imports: [
        MatIconModule, 
    ],
    templateUrl: './user-select.html',
    styleUrl: './user-select.scss',
})
export class UserSelect {
    readonly cascadingService = inject(CascadingService); 

    private readonly colors = [
        '#F44336', '#E91E63', '#9C27B0', '#673AB7', 
        '#3F51B5', '#2196F3', '#009688', '#4CAF50', 
        '#FF9800', '#FF5722'
    ];
    
    getUserColor(id: string): string {
        let hash = 0;
        
        for (let i = 0; i < id.length; i++) {
            hash = id.charCodeAt(i) + ((hash << 5) - hash);
        }

        return this.colors[Math.abs(hash) % this.colors.length];
    }
}

import { Injectable, inject, signal, PLATFORM_ID } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { isPlatformBrowser } from '@angular/common';


@Injectable({ 
    providedIn: 'root', 
})
export class SnackbarService {
    private readonly platformId = inject(PLATFORM_ID);
    private readonly snackBar = inject(MatSnackBar); 

    protected readonly errorMessage = signal('');
    
    showError(message: string) {
        this.errorMessage.set(message);

        if(isPlatformBrowser(this.platformId)){
            const ref = this.snackBar.open(message, 'Close', { duration: 4000 });

            ref.afterDismissed().subscribe(() => {
                this.errorMessage.set('');
            });       
        }
    } 
}
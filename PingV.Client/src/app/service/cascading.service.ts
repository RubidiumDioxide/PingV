import { Injectable, signal, effect, inject, PLATFORM_ID } from '@angular/core';
import { UserDto } from '../interfaces/user-dto.interface';
import { isPlatformBrowser } from '@angular/common';


@Injectable({
    providedIn: 'root'
})
export class CascadingService { 
    private readonly platformId = inject(PLATFORM_ID);

    users = signal<UserDto[]>([]);
    selectedUser = signal<UserDto | null>(null);

    constructor() {
        if(isPlatformBrowser(this.platformId)){
            const savedId = localStorage.getItem('selectedUserId');
            
            if (savedId) {
                const savedUser = this.users().find(u => u.id == savedId); 

                if(savedUser){
                    this.selectedUser.set(savedUser); 
                }
            }

            effect(() => {
                const userId = this.selectedUser()?.id; 
                
                if (userId) {
                    localStorage.setItem('selectedUserId', userId);
                } else {
                    localStorage.removeItem('selectedUserId');
                }
            });
        }
    }

    selectUser(id: string) {
        this.selectedUser.set(this.users().find(u => u.id == id) ?? null);  
    }
}
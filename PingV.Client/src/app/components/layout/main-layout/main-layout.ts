import { Component, OnInit, inject, signal, PLATFORM_ID } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from '../header/header';
import { Footer } from '../footer/footer';
import { UserRequestService } from '../../../requests/user-request-service';
import { CascadingService } from '../../../service/cascading.service';
import { MatChipsModule} from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'; 
import { MatIconModule } from '@angular/material/icon';
import { UserSelect } from '../../misc/user-select/user-select';
import { SnackbarService } from '../../../service/snackbar.service';
import { isPlatformBrowser } from '@angular/common';


@Component({
    selector: 'app-main-layout',
    imports: [ 
        RouterOutlet, 
        Header, 
        Footer, 
        MatChipsModule, 
        MatProgressSpinnerModule, 
        MatIconModule, 
        UserSelect, 
    ],
    templateUrl: './main-layout.html',
    styleUrl: './main-layout.scss',
})
export class MainLayout implements OnInit {
    private readonly platformId = inject(PLATFORM_ID);
    readonly cascadingService = inject(CascadingService); 
    readonly snackbarService = inject(SnackbarService); 

    private readonly userRequestService = inject(UserRequestService);

    protected readonly isLoading = signal(false);
    protected readonly errorMessage = signal('');

    // loading && operations 
    ngOnInit(): void {
        if(isPlatformBrowser(this.platformId)){
            this.getUsers();
        }
    }

    getUsers(): void {
        this.isLoading.set(true);
        this.userRequestService.get().subscribe({
            next: (data) => {
                this.cascadingService.users.set(data);
                this.isLoading.set(false);
            },
            error: (err) => {
                this.snackbarService.showError('Failed to load users.');
                this.isLoading.set(false);
            }
        });
    }
}

import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthStore } from '../stores/auth.store';

//Guards kontrollerar om en route får aktiveras eller lämnas
//Guarden returnerar ett boolean värde. Om det är true kommer guarden att låta användaren besöka routen
export const authGuard = () => {
    const authStore = inject(AuthStore);
    const router = inject(Router);

    if (authStore.isLoggedIn()) {
        return true;
    }

    return router.navigate(['/user/login']);
};
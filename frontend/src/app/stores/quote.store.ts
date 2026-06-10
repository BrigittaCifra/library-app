import { QuoteModel } from "../models/quote.model";
import { HttpErrorResponse, HttpHeaderResponse } from "@angular/common/http";
import { inject, Injectable, signal, computed } from "@angular/core";
import { QuoteService } from "../services/quote.service";

//En konstruktor är en funktion som körs när man skapar en ny instans, objekt, av en klass.
//Ett interface är custom data types
export interface QuoteState {
    quote: readonly QuoteModel[],
    loading: boolean,
    error: HttpErrorResponse | null
}

//interface kan inte ha default värden. Därför måste de få default värden på det här sättet
const defaultState: QuoteState = {
    quote: [],
    loading: false,
    error: null
}

@Injectable({ providedIn: 'root' })
export class QuoteStore {

    //dependency injection. Moderna sättet att hämta saker på istället för att använda konstruktorn
    //inject hämtar instansen av BookService
    private readonly quoteService = inject(QuoteService);
    private state = signal(defaultState);

    //computed() skapar en signal som automatiskt räknar ut sitt värde baserat på en annan
    //computed låter komponenter läsa book arrayen på ett säkert sätt
    quote = computed(() => this.state().quote);
    loading = computed(() => this.state().loading);
    error = computed(() => this.state().error);

    //state uppdateringar
    //loading
    private setLoading() {
        //spread operatorn kopierar alla gamla state och sätter loading till true
        this.state.update(() => ({ ...this.state(), loading: true }));
    }

    //error
    private setError(error: HttpErrorResponse) {
        this.state.update(() => ({ ...this.state(), loading: false, error }));
    }

    //Hämtar alla böcker
    loadQuote() {
        //sätter loadingen till true
        this.setLoading();

        //skickar http get request BookService
        //subscribe gör operationen async
        this.quoteService.getQuote()
            .subscribe({
                next: (quote) => {
                    //spread operatorn kopierar alla gamla state och bok arrayen uppdateras med svaret från API:et
                    this.state.update(() => ({ ...this.state(), loading: false, quote }))
                },
                error: (error) => {
                    //loggar ut felmeddelandet till konsolen
                    console.log(error);
                    this.setError(error);
                }
            });
    }

    //Tar bort ett citat
    deleteQuote(id: number) {
        this.quoteService.deleteQuote(id)
            .subscribe({
                next: () => {
                    //spread operatorn kopierar alla gamla state och bok arrayen uppdateras med svaret från API:et
                    this.state.update(() => ({ ...this.state(), loading: false, quote: this.quote().filter(q => q.id !== id) }))
                },
                error: (error) => {
                    //loggar ut felmeddelandet till konsolen
                    console.log(error);
                    this.setError(error);
                }
            });
    }

    constructor() {
        this.loadQuote();
    }

}
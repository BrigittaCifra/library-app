import { HttpErrorResponse, HttpHeaderResponse } from "@angular/common/http";
import { inject, Injectable, signal, computed } from "@angular/core";

//Bok relaterade imports
import { BookModel } from "../models/book.model";
import { BookService } from "../services/book.service";

//En konstruktor är en funktion som körs när man skapar en ny instans, objekt, av en klass.
//Ett interface är custom data types
export interface BookState {
    book: readonly BookModel[],
    loading: boolean,
    error: HttpErrorResponse | null
}

//interface kan inte ha default värden. Därför måste de få default värden på det här sättet
const defaultState: BookState = {
    book: [],
    loading: false,
    error: null
}

@Injectable({ providedIn: 'root' })
export class BookStore {
    //dependency injection. Moderna sättet att hämta saker på istället för att använda konstruktorn
    //inject hämtar instansen av BookService
    private readonly BookService = inject(BookService);
    private state = signal(defaultState);

    //computed() skapar en signal som automatiskt räknar ut sitt värde baserat på en annan
    //computed låter komponenter läsa book arrayen på ett säkert sätt
    book = computed(() => this.state().book);
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
    loadBook() {
        //sätter loadingen till true
        this.setLoading();

        //skickar http get request BookService
        //subscribe gör operationen async
        this.BookService.getBook()
            .subscribe({
                next: (book) => {
                    //spread operatorn kopierar alla gamla state och bok arrayen uppdateras med svaret från API:et
                    this.state.update(() => ({ ...this.state(), loading: false, book }))
                },
                error: (error) => {
                    //loggar ut felmeddelandet till konsolen
                    console.log(error);
                    this.setError(error);
                }
            });
    }

    //Skapar en ny bok
    addBook(book: BookModel) {
        this.BookService.addBook(book)
            .subscribe({
                next: (book: BookModel) => {
                    //spread operatorn kopierar alla gamla state och bok arrayen uppdateras med svaret från API:et
                    //book: [...this.book(), book] <- appending in the array
                    this.state.update(() => ({ ...this.state(), loading: false, book: [...this.book(), book] }))
                },
                error: (error) => {
                    //loggar ut felmeddelandet till konsolen
                    console.log(error);
                    this.setError(error);
                }
            });
    }

    //uppdaterar en bok
    updateBook(book: BookModel) {
        this.BookService.updateBook(book)
            .subscribe({
                next: () => {
                    //spread operatorn kopierar alla gamla state och bok arrayen uppdateras med svaret från API:et
                    this.state.update(() => ({ ...this.state(), loading: false, book: this.book().map(b => b.id === book.id ? book : b) }))
                },
                error: (error) => {
                    //loggar ut felmeddelandet till konsolen
                    console.log(error);
                    this.setError(error);
                }
            });
    }

    //Tar bort en bok
    deleteBook(id: number) {
        this.BookService.deleteBook(id)
            .subscribe({
                next: () => {
                    //spread operatorn kopierar alla gamla state och bok arrayen uppdateras med svaret från API:et
                    this.state.update(() => ({ ...this.state(), loading: false, book: this.book().filter(b => b.id !== id) }))
                },
                error: (error) => {
                    //loggar ut felmeddelandet till konsolen
                    console.log(error);
                    this.setError(error);
                }
            });
    }

    //Här används konstruktorn för datan laddas då in en gång när appen startar i kombination med providedIn: 'root'
    constructor() {
        this.loadBook();
    }

}
# Bookly
Bookly är en CRUD applikation som hjälper användare att hålla koll på sina favorit böcker och citat. 

## 📖 Beskrivning
Bookly har CRUD funktionalitet via följande endpoints;

Böcker:
- GET ``/book`` för att hämta alla böcker
- GET ``/book/:id`` för att hämta en specifik bok
- POST ``/book`` för att skapa en ny bok
- PUT ``/book/:id`` för att uppdatera en befintlig bok
- DELETE ``/book/:id`` för att ta bort en bok

Citat:
- GET ``/quote`` för att hämta alla citat
- GET ``/quote/:id`` för att hämta ett specifikt citat
- POST ``/quote`` för att skapa en ny citat
- PUT ``/quote/:id`` för att uppdatera en befintlig citat
- DELETE ``/quote/:id`` för att ta bort ett citat

Användare:
- POST ``/user/register`` för att skapa en ny användare
- POST ``/user/login`` för att logga in på ett befintligt användarkonto 
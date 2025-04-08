# **SimplyBooks API - README**

## **Project Overview**

**SimplyBooks** is an API-based web application designed to manage a collection of books and authors. The system provides full CRUD (Create, Read, Update, Delete) functionality for both authors and books, allowing users to interact with the system to add, update, delete, and view information about books and authors. Users can also filter and retrieve favorite authors and books associated with them.

This API is built using **ASP.NET Core** and **Entity Framework Core** with a **PostgreSQL** database. The project supports both HTTP GET and POST calls for retrieving and adding records, as well as PUT and DELETE methods for updating and removing records.

---

## **Database Configuration**

The project uses **PostgreSQL** for the database and **Entity Framework Core** for ORM (Object-Relational Mapping). The key models are `Author` and `Book`, with a one-to-many relationship between the two (an author can have many books). Below is an example of how the database schema is structured:

### **Authors Table**

| Column Name | Type    | Description                                  |
|-------------|---------|----------------------------------------------|
| Id          | Integer | Primary Key                                  |
| FirstName   | String  | Author's first name                          |
| LastName    | String  | Author's last name                           |
| Email       | String  | Author's email address                       |
| Image       | String  | File name of the author's image              |
| Favorite    | Boolean | Flag to mark favorite authors                |
| Uid         | String  | Unique Identifier for the author             |

### **Books Table**

| Column Name | Type    | Description                                      |
|-------------|---------|--------------------------------------------------|
| Id          | Integer | Primary Key                                      |
| Title       | String  | Title of the book                                |
| Description | String  | Description of the book                          |
| AuthorId    | Integer | Foreign Key referencing the Authors table        |
| Author      | Author  | Navigation property (EF Core relationship)       |
| Image       | String  | File name for the book's image                   |
| Price       | Decimal | Price of the book                                |
| Sale        | Boolean | Flag indicating if the book is on sale           |
| Uid         | String  | Unique Identifier for the book                   |

---

## **API Endpoints**

### **Authors Endpoints**

- **GET /authors**: Returns a list of all authors.
- **GET /authors/{id}**: Fetch an author by their `id`.
- **GET /authors/favorite**: Fetch a list of authors marked as favorite.
- **GET /authors/{authorId}/books**: Fetch a list of a specific author's books.
- **POST /authors**: Creates a new author.
- **PUT /authors/{id}**: Updates an existing author.
- **DELETE /authors/{id}**: Deletes an author.

### **Books Endpoints**

- **GET /books**: Returns a list of all books.
- **GET /books/{id}**: Fetch a book by its `id`.
- **POST /books**: Creates a new book.
- **PUT /books/{id}**: Updates an existing book.
- **DELETE /books/{id}**: Deletes a book.

---

## **Postman Docs**

You can interact with and explore the SimplyBooks API using the official Postman documentation:

[Postman Documentation](https://documenter.getpostman.com/view/36672102/2sB2cVf2FP)

---

## **Link to GitHub Repository**

The source code for this project is available on GitHub:

[SimplyBooks GitHub Repository](https://github.com/DTrejo98/Simply-Books-BE.git)


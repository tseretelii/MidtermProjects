# MidtermProjects

This repository contains four distinct C# projects that demonstrate fundamental concepts of programming, including object-oriented programming, user input validation, and file handling. Each project has been designed to be interactive and user-friendly. This repository was created as part of IT Step Academy's C# course midterm assignment.

---

## Table of Contents

1. [Overview](#overview)
2. [Projects](#projects)
   - [1. Calculator](#1-calculator)
   - [2. Guessing Game](#2-guessing-game)
   - [3. Hangman](#3-hangman)
   - [4. Book Manager](#4-book-manager)
3. [How to Run](#how-to-run)
4. [Features](#features)
5. [Technologies Used](#technologies-used)
6. [Contributing](#contributing)
7. [License](#license)

---

## Overview

This repository demonstrates four interactive console-based applications, built using C#. These projects highlight different programming techniques and aim to provide a hands-on learning experience. Each project includes user input validation, clean code structure, and thoughtful functionality.

---

## Projects

### 1. Calculator

A console-based calculator application that allows users to perform:
- Single operations (addition, subtraction, multiplication, and division).
- Multiple sequential operations, maintaining a running total.

**Features:**
- User-friendly interface.
- Input validation to ensure error-free calculations.
- Handles division by zero gracefully.

---

### 2. Guessing Game

A fun, number-guessing game where:
- The system generates a random number within a user-specified range.
- Players attempt to guess the number with hints provided ("Higher!" or "Lower!") after each attempt.

**Features:**
- Random number generation.
- Feedback for each guess.
- Tracks the number of attempts.

---

### 3. Hangman

The classic Hangman game with selectable difficulty levels (Easy, Medium, Hard):
- The player guesses letters to reveal a hidden word.
- Each incorrect guess reduces the number of tries and progresses the hangman drawing.

**Features:**
- Predefined words with hints.
- ASCII art representation of the hangman.
- Dynamic feedback on guessed letters and remaining attempts.

---

### 4. Book Manager

A simple book management system that:
- Stores book details (title, author, and release year) in text files.
- Allows users to add, search, and list books.

**Features:**
- File handling for data persistence.
- Case-insensitive search by title.
- Organized display of all books with relevant details.

---

## How to Run

### Prerequisites
- Install [.NET SDK](https://dotnet.microsoft.com/download) on your system.

### Steps
1. Clone this repository:
   ```bash
   git clone https://github.com/your-username/MidtermProjects.git
   cd MidtermProjects
   ```
2. Open the project in your favorite IDE (e.g., Visual Studio or Visual Studio Code).
3. Build and run the project:
   ```bash
   dotnet run
   ```
4. Select the desired project by uncommenting the corresponding section in the `Program.cs` file.

---

## Features

- **Calculator:** Perform arithmetic operations seamlessly.
- **Guessing Game:** Test your intuition with random number guessing.
- **Hangman:** Enjoy a challenging word-guessing game.
- **Book Manager:** Manage and search book records efficiently.

---

## Technologies Used

- Language: **C#**
- Platform: **.NET Core**
- Tools: Visual Studio, Visual Studio Code

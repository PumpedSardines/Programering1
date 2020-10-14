/*
    Author: Fritiof Rusck, 2020-09-22
    Description: A Program that runs hangman
*/
using System;
using System.Collections.Generic; // Because lists are being used
using System.IO; // We load a random word from a file, therfore we need to be able to read files

// Hangman namespace covers all the hangman logic
namespace Hangman {
    // Letter class, sotres what letter it is and if it's hidden or not. 
    public class Letter {
        public char letter { get; set; }
        public bool hidden { get; set; }
        public Letter(char _letter, bool _hidden) {
            letter = _letter;
            hidden = _hidden;
        }
    }

    // The Hangman game class that stores a hangman game and covers all hangman logic
    class Game {
        public Letter[] word; // Stores the word the guesser needs to guess
        public List<char> guessedLetter = new List<char>(); // Stores which letters the guesser has guessed
        public int incorrectGuesses = 0; // Stores how many incorrect guesses has been made

        // Constructor that takes a string as the word the guesser needs to guess
        public Game(string _word) {
            // Convert the _word string and converts it into a char array
            char[] letters = _word.ToCharArray();
            word = new Letter[letters.Length];

            // Crate a new letter instance with the char arrays as letters, set hidden to true since no letters has been guessed yet
            for(int i = 0; i < letters.Length; i++) {
                word[i] = new Letter(letters[i],true);
            }
        }

        // Returns if all words are shown, if that's true return true else return false
        public bool hasGameEnded() {
            foreach(Letter letter in word) {
                if(letter.hidden) return false;
            }
            return true;
        }

        // Guess a letter, if the letter is correct reveal those letters, otherwise increment incorrectGuesses. 
        // Returns if you can guess that letter or not
        public bool guessLetter(char guessingLetter) {
            // If letter is already guessed return
            if(guessedLetter.Contains(guessingLetter)) {
                return false;
            }

            // Finds and updates every charcter
            bool hasChange = false;
            foreach(Letter letter in word) {
                if(letter.letter == guessingLetter) {
                    letter.hidden = false;
                    hasChange = true;
                }
            }

            // Adds letter to guessed letters
            guessedLetter.Add(guessingLetter);

            // If guess didn't match add one to incorrect guesses
            if(!hasChange) {
                incorrectGuesses++;
            }

            return true;
        }
    }

    /*
        ConsoleRenderer class, used to print a Game class,
        i've decided to create a new class for this since the render proccess isn't a part of the game logic
        This also means i can have multiple ways to render ta game class in the future, for example: i want to draw it to a window.
    */
    class ConsoleRenderer {
        string msg = ""; // Stores if a message should be shown for the user, example: "That letter has already been picked"

        // I like to set my variables trough functions to controll more exactly what is being set, for example: if i would like to update a value everytime i set the message i can do it in this function
        public void setMessage(string _msg) {
            msg = _msg;
        }

        /*
            This function prints a Game class to the console.

            I use a variable "printString" to print everything that's needed at the same time
            Since stdout is slow i'd like to use Console.WriteLine once, 
            so instead of using it everytime i need a new line i instead add a newline character to printString
        */
        public void printGame(Game game) {
            Console.Clear();
            string printString = msg += "\n";

            foreach(Letter letter in game.word) {
                if(letter.hidden) {
                    printString += "_ ";
                }else {
                    printString += letter.letter.ToString() + " ";
                }
            }
            printString += '\n';

            foreach(char letter in game.guessedLetter) {
                printString += letter.ToString() + " ";
            }
            printString += "\n\n";
            printString += "Times guessed: " + game.incorrectGuesses.ToString();

            Console.WriteLine(printString);
            msg = ""; // set message to "" since it should only be displayed once every render. 
            // A better aproach would be to create a function removeMessage() and call that instead, but i tought this was enough for such a small project
        }
    }
}



namespace Program {
    class Program {
        // gets a random word from word.txt in the same folder
        public static string getRandomWord() {
            string[] lines = File.ReadAllLines("words.txt");  // read all lines
            Random rnd = new Random();
            int value = rnd.Next(0,lines.Length); // pick a random line
            return lines[value]; // return value of that line
        }

        static void Main(string[] args) {
            Hangman.Game game = new Hangman.Game(getRandomWord()); // Create a new instance of game with a random word
            Hangman.ConsoleRenderer ren = new Hangman.ConsoleRenderer(); // Create a new renderer to render tha hangman game
            while(true) { // a loop to keep the game running
                ren.printGame(game); // start every game loop with render the game
                string guessedLetter = Console.ReadLine(); // store the guessed letter in a string t;
                /* 
                    since t is a string and guessLetter take a char as value i pick the first element in the string, 
                    this makes sense since if i inputed: "abc" i wouldn't be able to guess that anyways, so it guesses 'a' instead
                */
                if(!game.guessLetter(guessedLetter[0])) { // Guess the letter and check if that letter could be guessed, if not set message too: "Du har redan valt den bokstaven"
                    ren.setMessage("Du har redan valt den bokstaven"); 
                } 
                if(game.hasGameEnded()) { // if game has ended break the loop and print the game on last time with a victory message
                    ren.setMessage("Du vann!");
                    ren.printGame(game);
                    break;
                }
            }
        }
    }
}
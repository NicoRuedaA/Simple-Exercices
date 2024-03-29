﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Compression;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;

namespace HyperSpaceCheeseGame // Note: actual namespace depends on the project name.
{


    class Player
    {
        public string name { get; set; }
        public string token { get; set; }
        public Vector2 vec { get; set; }

        public Player()
        {
            name = " ";
            token = " ";
            vec = new Vector2(-1, -1);
        }

        public Player(string n, string t, Vector2 v)
        {
            name = n;
            token = t;
            vec = v;
        }

        public Player(string n)
        {
            name = n;
            token = " ";
            vec = new Vector2(-1, -1);
        }

    }

    internal class Program
    {



        static Vector2 winPos = new Vector2(7, 7);


        const int TABLE_HIGH = 8, TABLE_LENGTH = 8;

        static int[,] _board = new int[,] {
            {3, 4, 4, 4, 4, 4, 4, 1},

            {3, 4, 4, 4, 4, 4, 4, 4},

            {3, 3, 3, 3, 3, 4, 3, 4},

            {3, 1, 4, 4, 4, 4, 1, 4},

            {3, 3, 2, 2, 3, 3, 3, 4},

            {3, 3, 4, 3, 3, 3, 4, 4},

            {3, 2, 2, 2, 2, 2, 2, 1},

            {3, 2, 2, 2, 2, 2, 2, 0}
        };


        static Vector2 cheesePosition = new Vector2(0, 4);


        static string[,] boardTable = new string[TABLE_LENGTH, TABLE_HIGH];

        static string readString(string prompt)
        {
            string result;
            do
            {
                Console.Write(prompt);
                result = Console.ReadLine();
            } while (result == "");
            return result;
        }


        static char readChar(string prompt)
        {
            string stringAux;
            stringAux = readString(prompt);
            char[] characters = stringAux.ToCharArray();

            return characters[0];
        }


        static int readInt(string prompt)
        {
            int result = -1;
            do
            {
                try
                {
                    string intString = readString(prompt);
                    result = int.Parse(intString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Only numbers please");
                }
            } while (result is < 0 or > 4);
            return result;
        }


        static void DecideIfMove(out char _selectionCrit)
        {
            do
            {
                _selectionCrit = readChar("Wanna move? (N/S)");
            } while ((_selectionCrit != 'N') && (_selectionCrit != 'S'));
        }


        static void PrepareGame(ref Player[] players)
        {
            PreparePlayers(ref players);

            for (int i = 0; i < TABLE_HIGH; i++)
            {
                for (int j = 0; j < TABLE_LENGTH; j++)
                {
                    boardTable[i, j] = GetCellType(new Vector2(i, j));
                }
            }
        }

        static void PreparePlayers(ref Player[] players)
        {

            int numPlayers = readInt("How many players?: ");
            string playerName;
            players = new Player[numPlayers];

            for (int i = 0; i < players.Length; i++)
            {

                // Crea una nueva instancia de Player y asigna la propiedad name
                playerName = readString($"Insert player {i} name: ");
                CreatePlayer(ref players[i], playerName);
            }
        }

        static void CreatePlayer(ref Player player, string name)
        {
            player = new Player(name);
            player.token = player.name[0].ToString();
            player.vec = new Vector2(-1, -1);
        }

        static string GetCellType(Vector2 pos)
        {
            string result;
            switch (_board[(int)pos.X, (int)pos.Y])
            {
                case 1:
                    result = "↓";
                    break;
                case 2:
                    result = "←";
                    break;
                case 3:
                    result = "↑";
                    break;
                case 4:
                    result = "→";
                    break;
                case 0:
                    result = "W";
                    break;
                default:
                    result = "?";
                    break;
            }
            return result;
        }


        static bool CheckIfWin(ref Player player)
        {
            if (player.vec == winPos)
            {
                Console.WriteLine("Game ENDED");
            }
            return player.vec == winPos;
        }



        private static void PlayRound(Player[] players, ref bool end)
        {
            for (int i = 0; i < players.Length; i++)
            {
                PlayerTurn(ref players, ref players[i]);
                if (CheckIfWin(ref players[i]))
                {
                    end = true;
                    break;
                }
            }
            string aux = readString("Enter any character for continue: ");
        }


        static void PrintTable()
        {


            for (int j = TABLE_HIGH - 1; j >= 0; j--)
            {
                for (int i = 0; i < TABLE_LENGTH; i++)
                {
                    Console.Write(boardTable[i, j].PadRight(5));
                }
                Console.WriteLine();
            }
        }

        static void UpdateTable(Player[] players)
        {
            foreach (var player in players)
            {
                if (player.vec == new Vector2(-1, -1)) continue;
                Console.WriteLine($"{player.vec} {player.name}");
                RefreshGUI(player);
            }
        }



        static void PlayerTurn(ref Player[] players, ref Player actualPlayer)
        {
            int diceResult = 2;
            //RollDices(actualPlayer);
            Move(ref players, ref actualPlayer, diceResult);

        }


        static int RollDices(Player player)
        {
            int min = 1, max = 7, result;
            Random random = new Random();
            result = random.Next(min, max);
            Console.WriteLine($"{player.name} roll result is " + result);
            return result;
        }

        private static void Move(ref Player[] players, ref Player actualPlayer, int diceResult)
        {

            Vector2 newPos;

            //si no está inicializado iniciamos el juego en 0,0
            if (actualPlayer.vec == new Vector2(-1, -1))
            {
                MoveTo(ref actualPlayer, new Vector2(0, 0));
            }

            Console.WriteLine($"Player {actualPlayer.name} position actual is {actualPlayer.vec}");
            Console.WriteLine($"{actualPlayer.vec} is a cell of type {_board[(int)actualPlayer.vec.X, (int)actualPlayer.vec.Y]}");

            int actualPosCellType = _board[(int)actualPlayer.vec.X, (int)actualPlayer.vec.Y];
            Vector2 direction = new Vector2(0, 0);
            switch (actualPosCellType)
            {
                case 1:
                    direction = new Vector2(0, -1);
                    break;
                case 2:
                    direction = new Vector2(-1, 0);
                    break;
                case 3:
                    direction = new Vector2(0, 1);
                    break;
                case 4:
                    direction = new Vector2(1, 0);
                    break;
                default:
                    newPos = new Vector2(0, 0);
                    Console.WriteLine("ERROR. Invalid Cell tpye");
                    break;
            }

            newPos = actualPlayer.vec + direction * diceResult;
            if (newPos.Y <= 0) newPos.Y = 0;
            else if (newPos.X < 0) newPos.X = 0;
            else if (newPos.Y >= TABLE_HIGH) newPos.Y = TABLE_HIGH - 1;
            else if (newPos.X >= TABLE_LENGTH) newPos.X = TABLE_LENGTH - 1;

            //MoveTo(ref actualPlayer, newPos);

            CheckIfCellisEmpty(ref players, ref actualPlayer, ref newPos);

            //RefreshGUI(actualPlayer);
            MoveTo(ref actualPlayer, newPos);
            Console.WriteLine($"New pos of {actualPlayer.name} is {actualPlayer.vec}");
            if (actualPlayer.vec == cheesePosition) CheeseWiski(ref players, actualPlayer);
            ShowTable(players);

        }


        static void ShowTable(Player[] players)
        {
            //UpdateTable(players);
            PrintTable();
        }

        static void MoveTo(ref Player player, Vector2 newPosition)
        {
            if (player.vec != new Vector2(-1, -1)) DeleteOldGui(player.vec);
            player.vec = newPosition;
            RefreshGUI(player);
        }

        static void CheckIfCellisEmpty(ref Player[] players, ref Player actualPlayer, ref Vector2 cellPos)
        {
            Player toPush = new Player("aux");
            if (!IfisEmpty(ref players, ref actualPlayer, ref cellPos, ref toPush))
            {
                PushToken(ref players, ref toPush);
            }
        }


        static bool IfisEmpty(ref Player[] players, ref Player actualPlayer, ref Vector2 cellPos, ref Player toPush)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].token == actualPlayer.token)
                {
                    continue;
                }

                else if (players[i].vec == cellPos)
                {
                    Console.WriteLine($"Cell {cellPos} is not empty by {players[i].name} who is at {players[i].vec}");
                    Console.WriteLine($"Token of {players[i].name} moved");
                    toPush = players[i];
                    return false;
                }
            }
            return true;
        }

        static void PushToken(ref Player[] players, ref Player actualPlayer)
        {
            Console.WriteLine($"Position pre push of {actualPlayer.name} is {actualPlayer.vec}");

            Vector2 oldPos = actualPlayer.vec;
            Vector2 newPos;
            boardTable[(int)oldPos.X, (int)oldPos.Y] = GetCellType(oldPos);

            //si la ficha esta arriba del todo
            if (actualPlayer.vec.Y >= TABLE_HIGH - 1)
            {
                //si está en la esquian sup izquierda
                if (actualPlayer.vec.X == 0)
                {

                    newPos = new Vector2(actualPlayer.vec.X + 1, actualPlayer.vec.Y);

                    Console.WriteLine($"token of {actualPlayer.name} pushed RIGHT");

                }
                else
                {
                    newPos = new Vector2(actualPlayer.vec.X - 1, actualPlayer.vec.Y);

                    Console.WriteLine($"token of {actualPlayer.name} pushed LEFT");
                }
            }
            else
            {

                newPos = new Vector2(actualPlayer.vec.X, actualPlayer.vec.Y + 1);

                Console.WriteLine($"token of {actualPlayer.name} pushed  UP");
            }

            //RefreshGUI(actualPlayer);
            CheckIfCellisEmpty(ref players, ref actualPlayer, ref newPos);

            Console.WriteLine($"Position Post push is {newPos}");
            MoveTo(ref actualPlayer, newPos);

            Console.WriteLine($"Position post push of {actualPlayer.name} is {actualPlayer.vec}");
            //if (newPos == cheesePosition) CheeseWiski(ref players, actualPlayer);
            //MoveTo(ref actualPlayer, newPos);
        }


        //FUNCION ORDENAR JUGADORES POR TOKEN, NOMBRE Y POSICION X Y POSIXION Y

        static Player GetPlayerAtCell(Player[] players, Vector2 cellPosition)
        {
            Player toReturn = new Player("null");
            //ordenar por posicion
            //cambiar le pos por burbuja
            foreach (var player in players)
            {
                if (player.vec == cellPosition) toReturn = player;
            }
            return toReturn;
        }


        static bool SelectPlayer(char c, Player[] players, Player player, ref Player playerToMove)
        {
            for (int i = 0; i < players.Length; i++)
            {
                string myString = players[i].name;
                char[] charArray = myString.ToCharArray();
                if (players[i].name == player.name) continue;

                else if (c == charArray[0])
                {
                    playerToMove = players[i];
                    return true;
                }
            }
            return false;
        }

        static void CheeseWiski(ref Player[] players, Player actualPlayer)
        {
            cheesePosition = new Vector2(-2, -2);
            Console.WriteLine("CHEESE WISKI");


            Player playerToMove = new Player();
            SearchPlayer(players, ref playerToMove);

            Vector2 newPos;
            do
            {
                int column = readInt("enter column ");
                newPos = new Vector2(column, 0);

                //static bool IfisEmpty(ref Player[] players, ref Player player, ref Vector2 cellPos, ref Player toPush)
            } while (!IfisEmpty(ref players, ref actualPlayer, ref newPos, ref playerToMove));
            Console.WriteLine($"CHEESE WHISKIED {playerToMove.name} to {newPos}");
            MoveTo(ref playerToMove, newPos);
            PrintTable();
            //RefreshGUI(actualPlayer);

        }

        private static void SearchPlayer(Player[] players, ref Player playerToMove)
        {
            bool exit = false;
            string name;
            do
            {
                name = readString("Introduce name to cheese whiski :");
                exit = IsPlayer(players, ref playerToMove, name);
            } while (!exit);
        }


        static bool IsPlayer(Player[] players, ref Player playerToMove, string name)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].name == name)
                {
                    playerToMove = players[i];

                    return true;
                }
            }
            Console.WriteLine("Name no valid");
            return false;
        }


        static void RefreshGUI(Player actualPlayer)
        {
            boardTable[(int)actualPlayer.vec.X, (int)actualPlayer.vec.Y] = actualPlayer.token.ToString();
        }

        static void DeleteOldGui(Vector2 oldPos)
        {

            boardTable[(int)oldPos.X, (int)oldPos.Y] = GetCellType(oldPos);
        }

        static void DecideIfNewGame(out char c)
        {

            do
            {
                c = readChar("New game? (N/S)");
            } while ((c != 'N') && (c != 'S'));
        }


        static void Main()
        {
            //IMPONER QUE NO PUEDAN HABER 2 TOKENS IGUALES. GENERAR ALEAORIO SI COMIENZAN POR EL MISMO STRIGN HASTA GENERAR 1 VALOR DIFERENTE

            int numPlayers = 0;
            Player[] players = new Player[numPlayers];
            char critNewGame;
            bool gameEnded;
            do
            {
                gameEnded = false;
                PrepareGame(ref players);
                do
                {
                    //round = 1turn.player.
                    //2 players = 1 round of 2 turns
                    PlayRound(players, ref gameEnded);
                } while (!gameEnded);
                DecideIfNewGame(out critNewGame);
            } while (critNewGame != 'N');
        }

    }
}
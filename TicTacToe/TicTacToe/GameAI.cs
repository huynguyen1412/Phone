

namespace TicTacToe {
    
        public class GameAI {

            public enum boardState {
                emptyBoard, fullBoard, hasSpace
            };

            public enum value {
                oWins, draw, unclear, xWins, winner
            };

            public enum slotState {
                ownedByO = -1, ownedByNull, ownedByX
            };

            public enum opponent {
                X, O, human, machine
            };

            public int NumberOfColumns() { return NumOfCols; }
            public int NumberOfRows() { return NumOfRows; }
  
            private const int NumOfRows = 3;
            private const int NumOfCols = 3;
            private int slotCounter { get; set; }
            
            slotState[,] gameBoardSlots = new slotState[3,3];

            public void ResetBoard() {
	            for (int x=0; x < NumOfRows;x++) {
		            for (int y=0; y < NumOfCols; y++) {
			            gameBoardSlots[x,y] = slotState.ownedByNull;
                    }
                }

                slotCounter = 0;
            }

            public void SetBoardPositionToOwner(int x,  int y, slotState state)
            {
	            if ((x >=0 && x < NumOfRows) && (y >=0 && y < NumOfCols)) {
		            gameBoardSlots[x,y] = state;
                    if (state == slotState.ownedByNull) {
                         slotCounter--;
                    }
                    else {
                         slotCounter++;
                    }
	            }
            }

            public bool IsPositionEmpty(int x, int y)
            {
	            if ((x >=0 && x < NumOfRows) && (y >=0 && y < NumOfCols)) {
		            return (gameBoardSlots[x, y] == slotState.ownedByNull);
	            }
	
	            return false;
            }

          
            public value GenerateMove( opponent op, ref int bestRow, ref int bestColumn, value alpha, value beta)
            {
	            value state, results;
	            slotState owner;
	            opponent opposite;
	            int notNeeded=0;

	          //  ttt_count++;
	            // See if we have a win
	            if ((state = evaluate()) != value.unclear ) {
		          //  depth++;
		            return state;
	            }

	            // Get the opposite player for the recursive call
	            if (op == opponent.X) {
		            owner = slotState.ownedByX ;  opposite = opponent.O; state = alpha;
	            }
	            else {
		            owner = slotState.ownedByO; opposite = opponent.X; state = beta;
	            }

	            int x, y;
	
	            for (x=0; x < NumOfRows;x++) {
		            for (y=0; y < NumOfCols; y++) {
			            if (gameBoardSlots[x,y] == slotState.ownedByNull) {
				            SetBoardPositionToOwner(x, y, owner);
				            results = GenerateMove(opposite, ref notNeeded, ref notNeeded, alpha, beta);
				            SetBoardPositionToOwner(x, y, slotState.ownedByNull);

				            if ((op == opponent.X && results > state)||
					            (op == opponent.O && results < state)) {

					            if (op == opponent.X) {
						            alpha = state = results;
					            }
					            else {
						            beta = state = results;
					            }
					
					            bestRow = x; bestColumn = y; 
					
					            if (alpha >= beta) {
						            return state;
					            }
				            }
			            }
		            }
	            }
	
	            return state;
            }

            /*
             Determine if we have a winner.  Look at each slot and see if we have three of X or O in
             a row, there are a total of 8 patterns, 3 rows, 3 columns, and 2 diagonals.
            */
            public value evaluate()
            {
	            return evaluateHelper(opponent.X) == value.winner ? value.xWins : 
		               evaluateHelper(opponent.O) == value.winner ? value.oWins : 
			            // See if the board is filled and determine if it is a draw
			            (this.GetBoardState() == boardState.fullBoard) ? value.draw : value.unclear ; 
            }

            /*
             Determine if we have a winner in the rows, cols, or the diagonals.
             Returns:
             winner -  if there is a winner in one of the 8 possibilities
             unclear - if there is no winner.  This can happen prior to the board filling up and it is doing a search 
			            a win.
            */

            public boardState GetBoardState()
            {
	            if (slotCounter == NumOfCols*NumOfRows)
		            return boardState.fullBoard;
	
	            return boardState.hasSpace;
            }

            value evaluateHelper(opponent p) {
                int x, y;
                int threeInARow, threeInACol;
                slotState s;

                if (p == opponent.X)
                    s = slotState.ownedByX;
                else
                    s = slotState.ownedByO;

                // See if there is a win in the rows
                for (x = 0; x < NumOfRows; x++) {
                    threeInARow = 0; threeInACol = 0;
                    for (y = 0; y < NumOfCols; y++) {

                        if (gameBoardSlots[x, y] == s) {
                            ++threeInARow;
                        }

                        if (gameBoardSlots[y, x] == s) {
                            ++threeInACol;
                        }
                    }

                    // Do we have a winner?
                    if (threeInARow == NumOfRows || threeInACol == NumOfCols) {
                        return value.winner;
                    }
                }

                // Now check the diagonals
                // (0,0, 1,1, 2,2) or (0,2 1,1 2,0)
                if (gameBoardSlots[1,1] == s) {
                    if ((gameBoardSlots[0,0] == s && gameBoardSlots[2,2] == s) ||
                        (gameBoardSlots[0,2] == s && gameBoardSlots[2,0] == s)) {
                        return value.winner;
                    }
                }

                return value.unclear;
            }
		}
}

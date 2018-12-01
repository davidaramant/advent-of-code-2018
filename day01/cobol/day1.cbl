IDENTIFICATION DIVISION. 
PROGRAM-ID.  Day1 . 
AUTHOR. David Aramant. 
ENVIRONMENT DIVISION. 
INPUT-OUTPUT SECTION. 
FILE-CONTROL. 
    SELECT InputFile ASSIGN TO "input-cobol.txt"
               ORGANIZATION IS LINE SEQUENTIAL. 
 
DATA DIVISION. 
FILE SECTION. 
FD  InputFile. 
01  FrequencyChange.
    88  EndOfInputFile VALUE HIGH-VALUES.
    02  Change         PIC S9(6) SIGN LEADING SEPARATE.
 
WORKING-STORAGE SECTION. 
01  WorkTotals. 
    02  Frequency        PIC S9(6) VALUE ZERO.

PROCEDURE DIVISION. 
Begin. 
    OPEN INPUT InputFile 

    READ InputFile 
      AT END SET EndOfInputFile TO TRUE 
    END-READ 
    PERFORM UNTIL EndOfInputFile 
       ADD Change TO Frequency 
       READ InputFile 
         AT END SET EndOfInputFile TO TRUE 
       END-READ 
    END-PERFORM 
 
    DISPLAY Frequency.
 
    CLOSE InputFile 
    STOP RUN. 

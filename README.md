# QuizzApp

QuizzApp is a quizz-type game, where player is given to answer randomly selected question from chosen category. 
Player can choose how many questions will be present in a single game. During the game, the user can see how many questions he has already
answered and how many of them were correct. This game has two types of questions: 
* _Choice-based questions_ - user is given up to four different options to choose from
from which one is correct.
* _Text-based questions_ - player is only given an empty field, where he has to enter answer using his keyboard. 
Upon pressing "Submit", if the submitted answer was among the list of correct answers, he will score a correct point. 
Otherwise he will be shown the first correct answer. These types of question can have as many as possible correct answers.
It is also important to note, that answer of these questions are case insenstive and. E. g. if the correct answer was "Lithuania" and
user submitted "lITHuanIA" then it will count as correct.

Every question can also have a single image attached to it. The image can be used for quesiton (e. g. "Who is this person" 
where the persons photo is attached to it) or just as a decoration.

User can also create his own categories and questions. 
* _Choice-based questions_ - these questions can have up to 4 possible choices, from which 1 must be correct.
* _Text-based questions_ - these questions only have 1 field "Correct answer" where creator has to enter all correct answers separated
by semicolon ';' (e. g. "United States of America;america;us;usa"). If the player answers this question incorrectly, only the first correct
answer will be shown.

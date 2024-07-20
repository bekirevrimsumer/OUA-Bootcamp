# The script of the game goes in this file.

# Declare characters used by this game. The color argument colorizes the
# name of the character.

define s = Character("Narrator")

#Declare music


# The game starts here.

label start:

    # Show a background. This uses a placeholder by default, but you can
    # add a file (named either "bg room.png" or "bg room.jpg") to the
    # images directory to show it.

    scene bg room

    # This shows a character sprite. A placeholder is used, but you can
    # replace it by adding a file named "eileen happy.png" to the images
    # directory.

    show eileen happy

    # These display lines of dialogue.

    s "You've created a new Ren'Py game."

    s "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam feugiat erat libero, ac feugiat diam pharetra tincidunt.
    Nullam tellus sem, semper sit amet tincidunt ac, dapibus at mauris."

    menu:
        "Option 1":
            "You chose option 1."
        "Option 2":
            "You chose option 2."
        "Option 3":
            "You chose option 3."
        "Option 4 is a bit longer, a tiny tiny bit longer so the button is bigger but just a bit":
            "You chose option 4 which is a bit longer I admit."


    s "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam feugiat erat libero, ac feugiat diam pharetra tincidunt. Nullam tellus sem, semper sit amet tincidunt ac, dapibus at mauris. "
    s "Donec sit amet placerat risus. Nulla facilisi. Sed maximus nisi et nulla facilisis luctus. Proin velit purus, volutpat id lectus sed, scelerisque fringilla velit."
    # This ends the game.

    return

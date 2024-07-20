init offset = -1

transform disc_rotate:
    subpixel True
    parallel:
        rotate 0
        linear 200 rotate 360
        repeat


image sparkle:
    subpixel True
    "gui/main_menu.png"
    alpha 0.0
    pause renpy.random.randint(0, 10)
    linear 5.0 alpha 1.0
    linear 5.0 alpha 0.0

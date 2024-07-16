init python:

    g.button("pg2_1")
    g.condition("persistent.pg2_1")
    g.image("CGs/Page2/1.png")

    g.button("pg2_2")
    g.condition("persistent.pg2_2")
    g.image("CGs/Page2/2.png")

    g.button("pg2_3")
    g.condition("persistent.pg2_3")
    g.image("CGs/Page2/3.png")

    g.button("pg2_4")
    g.condition("persistent.pg2_4")
    g.image("CGs/Page2/4.png")

    g.button("pg2_5")
    g.condition("persistent.pg2_5")
    g.image("CGs/Page2/5.png")

    g.button("pg2_6")
    g.condition("persistent.pg2_6")
    g.image("CGs/Page2/6.png")

    g.button("pg2_7")
    g.condition("persistent.pg2_7")
    g.image("CGs/Page2/7.png")

    g.button("pg2_8")
    g.condition("persistent.pg2_8")
    g.image("CGs/Page2/8.png")

    g.button("pg2_9")
    g.condition("persistent.pg2_9")
    g.image("CGs/Page2/9.png")

screen galleryB:
    tag menu
    add "gui/gallery.png"
    hbox:
        yalign 0.5
        xalign 0.5

        use gallery_navigation
        grid 3 3:
            spacing 25

            # Row 1
            add g.make_button("pg2_1","CGs/preview/pg2_1.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg2_2","CGs/preview/pg2_2.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg2_3","CGs/preview/pg2_3.png", locked = "CGs/preview/locked.png")


            ## Row 2
            add g.make_button("pg2_4","CGs/preview/pg2_4.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg2_5","CGs/preview/pg2_5.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg2_6","CGs/preview/pg2_6.png", locked = "CGs/preview/locked.png")


            ## Row 3
            add g.make_button("pg2_7","CGs/preview/pg2_7.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg2_8","CGs/preview/pg2_8.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg2_9","CGs/preview/pg2_9.png", locked = "CGs/preview/locked.png")

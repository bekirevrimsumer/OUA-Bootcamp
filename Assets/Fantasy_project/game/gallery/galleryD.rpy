init python:
    g.button("pg4_1")
    g.condition("persistent.pg4_1")
    g.image("CGs/Page4/1.png")

    g.button("pg4_2")
    g.condition("persistent.pg4_2")
    g.image("CGs/Page4/2.png")

    g.button("pg4_3")
    g.condition("persistent.pg4_3")
    g.image("CGs/Page4/3.png")

    g.button("pg4_4")
    g.condition("persistent.pg4_4")
    g.image("CGs/Page4/4.png")

    g.button("pg4_5")
    g.condition("persistent.pg4_5")
    g.image("CGs/Page4/5.png")

    g.button("pg4_6")
    g.condition("persistent.pg4_6")
    g.image("CGs/Page4/6.png")

    g.button("pg4_7")
    g.condition("persistent.pg4_7")
    g.image("CGs/Page4/7.png")

    g.button("pg4_8")
    g.condition("persistent.pg4_8")
    g.image("CGs/Page4/8.png")

    g.button("pg4_9")
    g.condition("persistent.pg4_9")
    g.image("CGs/Page4/9.png")

screen galleryD:
    tag menu
    add "gui/gallery.png"
    hbox:
        yalign 0.5
        xalign 0.5

        use gallery_navigation
        grid 3 3:
            spacing 25

            # Row 1
            add g.make_button("pg4_1","CGs/preview/pg4_1.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg4_2","CGs/preview/pg4_2.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg4_3","CGs/preview/pg4_3.png", locked = "CGs/preview/locked.png")


            ## Row 2
            add g.make_button("pg4_4","CGs/preview/pg4_4.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg4_5","CGs/preview/pg4_5.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg4_6","CGs/preview/pg4_6.png", locked = "CGs/preview/locked.png")


            ## Row 3
            add g.make_button("pg4_7","CGs/preview/pg4_7.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg4_8","CGs/preview/pg4_8.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg4_9","CGs/preview/pg4_9.png", locked = "CGs/preview/locked.png")

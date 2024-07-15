init python:
    g = Gallery()

    g.button("pg1_1")
    g.condition("persistent.pg1_1")
    g.image("CGs/Page1/1.png")

    g.button("pg1_2")
    g.condition("persistent.pg1_2")
    g.image("CGs/Page1/2.png")

    g.button("pg1_3")
    g.condition("persistent.pg1_3")
    g.image("CGs/Page1/3.png")

    g.button("pg1_4")
    g.condition("persistent.pg1_4")
    g.image("CGs/Page1/4.png")

    g.button("pg1_5")
    g.condition("persistent.pg1_5")
    g.image("CGs/Page1/5.png")

    g.button("pg1_6")
    g.condition("persistent.pg1_6")
    g.image("CGs/Page1/6.png")

    g.button("pg1_7")
    g.condition("persistent.pg1_7")
    g.image("CGs/Page1/7.png")

    g.button("pg1_8")
    g.condition("persistent.pg1_8")
    g.image("CGs/Page1/8.png")

    g.button("pg1_9")
    g.condition("persistent.pg1_9")
    g.image("CGs/Page1/9.png")


screen galleryA:
    tag menu
    add "gui/gallery.png"
    hbox:
        yalign 0.5
        xalign 0.5

        use gallery_navigation

        grid 3 3:
            spacing 25

            ## Row 1
            add g.make_button("pg1_1","CGs/preview/pg1_1.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg1_2","CGs/preview/pg1_2.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg1_3","CGs/preview/pg1_3.png", locked = "CGs/preview/locked.png")


            ## Row 2
            add g.make_button("pg1_4","CGs/preview/pg1_4.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg1_5","CGs/preview/pg1_5.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg1_6","CGs/preview/pg1_6.png", locked = "CGs/preview/locked.png")


            ## Row 3
            add g.make_button("pg1_7","CGs/preview/pg1_7.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg1_8","CGs/preview/pg1_8.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg1_9","CGs/preview/pg1_9.png", locked = "CGs/preview/locked.png")

init python:
    g.button("pg3_1")
    g.condition("persistent.pg3_1")
    g.image("CGs/Page3/1.png")

    g.button("pg3_2")
    g.condition("persistent.pg3_2")
    g.image("CGs/Page3/2.png")

    g.button("pg3_3")
    g.condition("persistent.pg3_3")
    g.image("CGs/Page3/3.png")

    g.button("pg3_4")
    g.condition("persistent.pg3_4")
    g.image("CGs/Page3/4.png")

    g.button("pg3_5")
    g.condition("persistent.pg3_5")
    g.image("CGs/Page3/5.png")

    g.button("pg3_6")
    g.condition("persistent.pg3_6")
    g.image("CGs/Page3/6.png")

    g.button("pg3_7")
    g.condition("persistent.pg3_7")
    g.image("CGs/Page3/7.png")

    g.button("pg3_8")
    g.condition("persistent.pg3_8")
    g.image("CGs/Page3/8.png")

    g.button("pg3_9")
    g.condition("persistent.pg3_9")
    g.image("CGs/Page3/9.png")


screen galleryC:
    tag menu
    add "gui/gallery.png"
    hbox:
        yalign 0.5
        xalign 0.5

        use gallery_navigation
        grid 3 3:
            spacing 25

            # Row 1
            add g.make_button("pg3_1","CGs/preview/pg3_1.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg3_2","CGs/preview/pg3_2.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg3_3","CGs/preview/pg3_3.png", locked = "CGs/preview/locked.png")


            ## Row 2
            add g.make_button("pg3_4","CGs/preview/pg3_4.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg3_5","CGs/preview/pg3_5.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg3_6","CGs/preview/pg3_6.png", locked = "CGs/preview/locked.png")


            ## Row 3
            add g.make_button("pg3_7","CGs/preview/pg3_7.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg3_8","CGs/preview/pg3_8.png", locked = "CGs/preview/locked.png")
            add g.make_button("pg3_9","CGs/preview/pg3_9.png", locked = "CGs/preview/locked.png")

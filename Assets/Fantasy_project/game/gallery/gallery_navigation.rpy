screen gallery_navigation:
    vbox:
        spacing 20
        xoffset -100

        imagebutton auto "gui/button/gallerybutton1_%s.png"  action ShowMenu("galleryA")
        imagebutton auto "gui/button/gallerybutton2_%s.png"  action ShowMenu("galleryB")
        imagebutton auto "gui/button/gallerybutton3_%s.png"  action ShowMenu("galleryC")
        imagebutton auto "gui/button/gallerybutton4_%s.png"  action ShowMenu("galleryD")

        imagebutton auto "gui/button/return_%s.png"  action Return() yoffset -350 xoffset 1300

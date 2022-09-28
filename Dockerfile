FROM ubuntu-debootstrap

RUN useradd -ms /bin/bash unity
WORKDIR /home/unity
COPY builds/linux/UnityPlayer.so /home/unity/
COPY builds/linux/ball2D.x86_64 /home/unity/
COPY builds/linux/ball2D_Data /home/unity/ball2D_Data/


RUN chown -R unity:unity /home/unity/ball2D*

USER unity

RUN ["chmod", "+x", "/home/unity/ball2D.x86_64"]

CMD ["./ball2D.x86_64"]

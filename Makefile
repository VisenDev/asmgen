run: main
	./main

main: main.o
	gcc main.o -o main -e _start -lc

main.o: main.asm
	nasm main.asm -o main.o -fmachO64

main.asm: main.fs
	gforth main.fs > main.asm

.PHONY clean:
clean:
	@trash main.asm main.o main

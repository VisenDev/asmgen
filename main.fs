: asm_imports ( -- )
\    ." extern putchar" cr
;

: asm_putc ( -- )
    ." mov rdi, 1" cr           \ File descriptor 1 (stdout)
    ." mov rsi, buf" cr         \ Address of buffer
    ." mov rdx, 1" cr           \ Write 1 byte

    ." pop rax" cr
    ." mov [rsi], rax" cr       \ store contents of rax in char
    ." mov rax, 0x2000004" cr   \ macOS syscall: write
    ." syscall" cr
;

: asm_main ( -- )
    ." global _start" cr
    ." section .text" cr
    asm_imports
    ." _start:" cr
    ." align 16" cr cr           \ Align _start to a 16-byte boundary
;

: asm_add ( -- )
    ." pop rax" cr
    ." pop rbx" cr
    ." add rax, rbx" cr
    ." push rax" cr
;

: asm_push ( n -- )
    ." push " . cr
;

: asm_quit ( -- )
    ." xor rax, rax" cr
    ." ret" cr
    cr
    cr
    ." section .bss" cr
    ." buf resb 8" cr
;



asm_main
char 'A' asm_push
\ 0 asm_push
\ asm_add
asm_putc
asm_quit
bye

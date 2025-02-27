0 constant NIL_SECTION
1 constant TEXT_SECTION
2 constant BSS_SECTION
3 constant DATA_SECTION
NIL_SECTION variable current_section 

create section_names
  s" NIL_SECTION"   ,  \ Index 0
  s" TEXT_SECTION"  ,  \ Index 1
  s" BSS_SECTION"   ,  \ Index 2
  s" DATA_SECTION"  ,  \ Index 3

: section_name ( n -- )
    2 CELL * section_names + @ COUNT
;

current_section @ section_name type


: assert_section ( n -- )
    dup
    current_section @ <> if
        ." Asm section is incorrect, expected section " . cr
        ." found " current_section @ . cr
        abort
    then
;

: asm_imports ( -- ) ;

: asm_putc ( -- )
    ." mov rdi, 1" cr           \ File descriptor 1 (stdout)
    ." mov rsi, buf" cr         \ Address of buffer
    ." mov rdx, 1" cr           \ Write 1 byte

    ." pop rax" cr
    ." mov [rsi], rax" cr       \ store contents of rax in char
    ." mov rax, 0x2000004" cr   \ macOS syscall: write
    ." syscall" cr
;

    
: asm_start ( -- )
    ." global _start" cr
    ." section .text" cr
    cr
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


: asm_newline ( -- ) 
    10 asm_push
    asm_putc
;

: asm_bss ( -- )
    ." section .bss" cr
    ." buf resb 8" cr
;

: asm_stop ( -- )
    ." xor rax, rax" cr
    ." ret" cr
    cr
    cr
    asm_bss
;

asm_start
    char A asm_push
    1 asm_push
    asm_add
    asm_putc
    asm_newline
asm_stop

bye






 \ : section ( "name" -- )
 \      parse-name
 \      ." section "
 \      type
 \      cr
 \ ; 

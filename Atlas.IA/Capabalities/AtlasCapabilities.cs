using System;
using System.Collections.Generic;
using System.Text;


namespace Atlas.IA.Capabilities;

public static class AtlasCapabilities
{
    public const string Text = """
    Comandos disponíveis no Atlas:

    1. open_application
       Descrição:
       Abre uma aplicação no computador.

       Parâmetros:
       - application: string, obrigatório.
         Nome do executável ou caminho da aplicação.
    Text
    2. wait
       Descrição:
       Aguarda um período antes de continuar.

       Parâmetros:
       - milliseconds: int, obrigatório.
         Tempo de espera em milissegundos.

    3. write_text
       Descrição:
       Escreve texto na janela atualmente ativa.

       Parâmetros:
       - text: string, obrigatório.
         Texto que deve ser escrito.
    4  - click
    Descrição: realiza um clique esquerdo do mouse em uma coordenada da tela.
    Parâmetros:
      - x: int, obrigatório
      - y: int, obrigatório
    5 - move_mouse
    Descrição: move o cursor do mouse para uma coordenada absoluta da tela sem clicar.
    Parâmetros:
      - x: int, obrigatório
      - y: int, obrigatório
    6 - press_key
    Descrição: pressiona uma única tecla do teclado.
    Parâmetros:
      - key: string, obrigatório

        Teclas suportadas incluem:
          ENTER
          TAB
          ESC
          BACKSPACE
          DELETE
          HOME
          END
          PAGEUP
          PAGEDOWN
          UP
          DOWN
          LEFT
          RIGHT
          SPACE

    7 - hotkey
    Descrição: executa uma combinação de teclas.
    Parâmetros:
      - keys: string, obrigatório

    Formato:
      As teclas devem ser separadas por +.

    Exemplos:
      CTRL+C
      CTRL+V
      CTRL+A
      CTRL+S
      CTRL+SHIFT+S
      ALT+TAB
      ALT+F4
    """;
}
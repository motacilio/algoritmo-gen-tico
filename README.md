# Algoritmo Genético - Otimização da Função F6

Trabalho prático da disciplina de **Inteligência Artificial** (Prof. Celso Camilo).

## Objetivo
Maximizar a função de Schaffer F6 através de um Algoritmo Genético Canônico com representação binária e elitismo.

### Especificações do Algoritmo
- **Função Objetivo:** Schaffer F6
  $$F_6(x, y) = 0.5 - \frac{\sin^2\left(\sqrt{x^2 + y^2}\right) - 0.5}{\left(1.0 + 0.001(x^2 + y^2)\right)^2}$$
- **Domínio:** $x, y \in [-100, 100]$
- **Cromossomo:** 44 bits (22 bits para $x$ e 22 bits para $y$)
- **Seleção:** Roleta Viciada
- **Crossover:** 1 Ponto de Corte (Taxa de 65%)
- **Mutação:** Inversão de bit a bit (Taxa de 0.8%)
- **População:** 100 indivíduos
- **Critério de Sobrevivência:** Elitismo (preserva o melhor indivíduo)

## Como Executar
Com o [.NET SDK](https://dotnet.microsoft.com/) instalado, execute na pasta do projeto:

```bash
dotnet run
```

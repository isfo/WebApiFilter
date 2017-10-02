# WebApiFilter

Desenvolvida com a intenção de facilitar o filtro via URl.

**Segue lista de queries permitidas:**

**page**: Valores inteiros, indica qual página deseja carregar, deve ser utilizado em conjunto com **pagesize**, inicia a partir do **1**

**pagesize**: Valores inteiros, indica qual é o tamanho da página. **Ex:** Caso o **pagesize** seja **20** e a **page** seja **3**, irá retornar do **41º** ao **60º** registro.

**orderby**: Indica as colunas e tipo de ordenação da esquerda para a direita. **Ex:** orderby=id desc, nome asc, dtnascimento asc 

**where**: Define regras de filtragem a partir de expressões lógicas. **Ex:** where=nome contains e,sexo = M

#4 Casos de WHERE
| Expressões        | Ex                                                        |
| ----------------- | :-------------------------------------------------------: |
| <=                | dtnascimento <= 2017-09-26T11:35:25                       |



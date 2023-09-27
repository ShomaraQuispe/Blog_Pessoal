USE db_blogpessoal;
GO

INSERT INTO tb_postagens (Titulo, Texto, Data)
VALUES ('Postagem 01', 'Texto da Postagem 01', SYSDATETIMEOFFSET());
INSERT INTO tb_postagens (Titulo, Texto, Data)
VALUES ('Postagem 02', 'Texto da Postagem 02', SYSDATETIMEOFFSET());
INSERT INTO tb_postagens (Titulo, Texto, Data)
VALUES ('Postagem 03 - Atualizado', 'Texto da Postagem 03 - Atualizado', SYSDATETIMEOFFSET());
GO

SELECT * FROM tb_postagens;
GO
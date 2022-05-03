# API для модуля вычисления семантической близости документов

<br>
<br>

## Спецификация
Доступно 3 метода - GET (Ping) и POST (Calculate, GetRelevant). 

Метод **Ping** возвращает строку "Alive" и служит для проверки работоспособности контроллера.

Метод **Calculate** принимает в качестве входного параметра путь к json-файлу с проаннотированным корпусом и рассчитывает семантическую близость для всех пар документов, содержащихся в корпусе
Метод **GetRelevant** принимает в качестве входных параметров:
1. путь к json-файлу с проаннотированным корпусом
2. id документа, семантически близкие к которому документы необходимо найти в корпусе
Метод возвращает список наиболее релевантных документов с числовым значением семантической близости

<br>

> Метод **Calculate** возвращает значение по следующему запросу 

```
curl --location --request POST 'https://localhost:<port>/api/semanticsimilarity/calculate?annotation=<annotation>'
```

> Метод **GetRelevant** возвращает значение по следующему запросу 

```
curl --location --request POST 'https://localhost:<port>/api/semanticsimilarity/getrelevant?annotation=<annotation>&documentId=<documentId>'
```

> Спецификация Swagger доступна по адресу:

```
'https://localhost:<port>/index.html'
```

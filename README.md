# API для модуля вычисления семантической близости документов

<br>
<br>

## Спецификация
Доступно 3 метода - GET (Ping) и POST (CalculateSimilarity, GetRelevant). 

Метод **Ping** возвращает строку "Alive" и служит для проверки работоспособности контроллера.

Метод **CalculateSimilarity** рассчитывает семантическую близость для всех пар документов, содержащихся в корпусе, которых хранится в базе данных
Метод **GetRelevant** принимает в качестве входного параметра id документа, семантически близкие к которому документы необходимо найти в корпусе
Метод возвращает список наиболее релевантных документов с числовым значением семантической близости

<br>

> Метод **CalculateSimialrity** возвращает значение по следующему запросу 

```
curl --location --request POST 'https://localhost:<port>/api/semanticsimilarity/calculatesimilarity'
```

> Метод **GetRelevant** возвращает значение по следующему запросу 

```
curl --location --request POST 'https://localhost:<port>/api/semanticsimilarity/getrelevant?documentId=<documentId>'
```

> Спецификация Swagger доступна по адресу:

```
'https://localhost:<port>/index.html'
```

# API для модуля вычисления семантической близости документов

<br>
<br>

## Спецификация
Доступно 4 GET-метода: Ping, GetSimilarity, GetSimilarityWithNames и GetRelevant. 

Метод **Ping** возвращает строку "Alive" и служит для проверки работоспособности контроллера.

Метод **GetSimilarity** рассчитывает семантическую близость для всех пар документов, содержащихся в корпусе, который хранится в базе данных
Метод **GetSimilarityWithNames** также рассчитывает семантическую близость для всех пар документов, но, помимо структуры, возвращаемой методом **GetSimilarity**, возвращает еще и названия всех документов, содержащихся в корпусе
Метод **GetRelevant** принимает в качестве входного параметра id документа, семантически близкие к которому документы необходимо найти в корпусе
Метод возвращает список наиболее релевантных документов с числовым значением семантической близости

<br>

> Метод **GetSimilarity** возвращает значение по следующему запросу 

```
curl --location --request GET 'https://localhost:<port>/api/semanticsimilarity/getsimilarity'
```

> Метод **GetSimilarityWithNames** возвращает значение по следующему запросу 

```
curl --location --request GET 'https://localhost:<port>/api/semanticsimilarity/getsimilaritywithnames'
```

> Метод **GetRelevant** возвращает значение по следующему запросу 

```
curl --location --request GET 'https://localhost:<port>/api/semanticsimilarity/getrelevant?documentId=<documentId>'
```

> Спецификация Swagger доступна по адресу:

```
'https://localhost:<port>/index.html'
```

Модуль опубликован по адресу: http://iksanovaal-001-site1.htempurl.com/index.html.

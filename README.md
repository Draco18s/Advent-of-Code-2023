
C# Solution for Advent of Code 2023

https://adventofcode.com/2023/

```
      --------Part 1---------   --------Part 2---------
Day       Time    Rank  Score       Time    Rank  Score
  7   00:29:31    2404      0   00:58:45    3546      0
  6   00:06:45    1146      0   00:09:53    1093      0
  5   00:35:28    3974      0   00:54:58    1174      0
  4   00:13:23    4821      0   00:19:41    2121      0
  3   00:15:20     859      0   00:31:47    1606      0
  2   17:56:18   89408      0   18:00:06   85016      0
  1       >24h  173958      0       >24h  129286      0
```

*Day 1, Day 2: Did not start on time again, surprise surprise.
*Day 4: Typo in the example tripped me up mentally for a good minute, then got the parameters on Aggregate backwards (redid as simple loop for the answer, then figured out why Aggregate wasn't working afterwards).
*Day 5: Part 1 was slowed by a sequence of minor errors, such as enum-parsing `"seedtosoilmap:"` instead of `"seedtosoilmap"`. Part 2 was pretty straight forward, but confused myself by not naming my anonymous tuples initially. Did beat both Stevie-O and Keith, though. Those two are beasts.
*Day 6: Finished part 2 before either Stevie-O or Keith finished part 1. That's...abnormal. This one was really pretty simple.
*Day 7: The `highcard` ordering was being a right pain and getting one case correct and another case wrong and had to resort to an alternate method until I could come back and examine things when I had time. Part 2 had an edge case I didn't properly account for, whoops. Slowed me down more than I care to admit.

C# Solution for Advent of Code 2023

https://adventofcode.com/2023/

```
      --------Part 1---------   --------Part 2---------
Day       Time    Rank  Score       Time    Rank  Score
 18   00:15:46     489      0   11:46:29   10120      0
 17   02:22:41    3058      0   18:58:58   13092      0
 16   00:43:50    2671      0   00:49:09    2226      0
 15   00:07:45    2641      0   00:21:22    1058      0
 14   00:04:33     140      0   00:12:31      25     76
 13   00:55:37    4348      0   01:25:54    3958      0
 12   23:31:52   29618      0          -       -      -
 11   00:13:54    1004      0   00:29:27    2085      0
 10   00:56:46    3966      0   01:29:37    1538      0
  9   00:05:57     332      0   00:10:49     746      0
  8   00:11:25    2857      0   00:52:43    4047      0
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
*Day 8: Did not recognize the LCM quick-solve in part 2 right away, then bungled submitting my answer, having inadvertently miscalculated said LCM.
*Day 9: <success_kid.jpg/> That's my second 332 rank (ties my 5th best rank since 2020; 3rd only counting part 1)
*Day 10: Was easier to turn the map [into an image and count isolated dots](https://cdn.discordapp.com/attachments/870341956734169158/1183295894058192926/image.png), but did a miscount ("five colors means 500, then the 55 extra..."). But it was sufficient to figure out how to do it via code. Though I did have to rewrite my Grid class's floodfill to be loopish instead of recursive.
*Day 12: Ugh. I gave this one a stab, but gave up.
*Day 13: Simple stupid mistake messed up my reflection-detection code during part 1. Part 2 took some trial-and-error to figure out which value was the value to use assuming a single smudge (i.e. I wouldn't always get a single non-zero value).
*Day 14: I think I got stupid lucky when I threw a 1000 iteration answer at the site (not the full 1000000000 cycles) and it was right. There was probably some pattern I was meant to find and instead finding it I tripped over the right answer. Better lucky than good?
*Day 16: Accidentally let beams phase through splitters if a beam had already been split by that splitter.
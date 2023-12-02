
C# Solution for Advent of Code 2022

https://adventofcode.com/2022/

```
      --------Part 1--------   --------Part 2--------
Day       Time   Rank  Score       Time   Rank  Score
 25   00:46:22   2015      0          -      -      -
 24   01:28:13   1666      0   01:36:36   1513      0
 23   01:20:20   2197      0   01:32:52   2232      0
 22   00:55:24   1371      0   03:56:35   1626      0
 21   00:56:15   4049      0   01:41:31   2981      0
 20   04:47:01   5412      0   04:59:24   4816      0
 19          -      -      0          -      -      -
 18   00:12:00   1616      0   00:17:08    285      0
 17   00:56:19   1226      0          -      -      -
 16          -      -      0          -      -      -
 15   00:41:54   2519      0          -      -      -
 14   00:43:27   3185      0   00:56:29   3230      0
 13   01:01:29   4285      0   01:11:14   3897      0
 12   01:10:40   4975      0   02:02:41   6673      0
 11   00:28:17   1471      0   01:22:13   4056      0
 10   00:07:07    359      0   00:16:08    348      0
  9   00:14:46    940      0   00:18:45    315      0
  8   00:12:43   1365      0   00:22:05   1093      0
  7   00:37:47   3096      0   00:48:40   3321      0
  6   00:04:08   1320      0   00:04:43    988      0
  5   00:27:02   4762      0   00:28:24   3821      0
  4   00:23:19   9971      0   00:25:28   8425      0
  3   00:09:48   2672      0   00:13:41   1817      0
  2   02:55:41  31150      0   03:03:09  28442      0
  1   00:03:24   1427      0   00:04:31   1033      0
```

*Day 1: Actually started on time for once.*
*Day 2: Started (2 hour 45 min) late because I misread the clock and got pulled into some Gunfire Reborn for 2 hours. It was worth it. Actual time taken: approximately 00:10:00, 00:08:00*
*Day 4: Started (14 min) late because I was distracted by Slipways*
*Day 5: I accidentally did part 2 for part 1 first (and had a few bugs in said).*
*Day 6: Lost about 30-45 seconds attempting an aproach that I abandoned as soon as I realized it was going to take longer than the method I went with in the end.*
*Day 11: Instructions were vauge about how to keep the numbers rational. I had the right idea at first, but due to the example data being what it was and because I was looking at the final values at the end in sorted order (not monkey order) I thought I was getting the wrong answer.*
*Day 12: There's no reason this should have taken me this long. Part 2, especially. I legitimately have no idea why part 2's initial attempt failed. Part 1 I overlooked that you could drop down distances further than 1 height unit and rewrote the pathfinder three times as a result.*
*Days 15,16,17,19: These were just too intense for me. Oof.*
*Day 20: Was playing Gunfire Reborn again, so progressed slowly through part 1. Eventually did a refactor over about a half hour that got me the right answers.*
*Day 21: Inadvertently went down a rabbit hole.*
*Day 22: Part 2 was a doozy. Converting to cublic coordinates ended up not being too bad (but errors lead me to doubting that math and repeatedly checking it). Ultimate problem ended up being the position of line 409 and the existence of 411-414. I had somehow convinced myself that I didn't actually have to check for valid movement after going around an edge any more...*
*Day 25: Little bit of a rabbit hole trying to convert to/from decimal before realizing that it was entirely unneeded.*
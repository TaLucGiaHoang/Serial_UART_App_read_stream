float buffer[2] = {0.5, 123.456};
static char *py;
void setup() {
  Serial.begin(115200);

//  Serial.print("float=");
//  Serial.println(sizeof(float)); // output: "float=4"
//  Serial.print("data_y=");
//  Serial.println(sizeof(data_y)); // output: "data_y=4"
//  Serial.print("buffer=");
//  Serial.println(sizeof(buffer)); // output: "buffer=8"
}

void loop() {
  py = (char*)buffer;
  for(int i = 0; i<1000; i++){
    Serial.write(py, sizeof(buffer) );
//    Serial.print(buffer[0]);Serial.print(", ");Serial.println(buffer[1]);
    buffer[0] = buffer[0] + 1.0;
    delay(30);
  }
  while(1);
}

/*
0.50, 123.46
1.50, 123.46
2.50, 123.46
3.50, 123.46
4.50, 123.46
5.50, 123.46
6.50, 123.46
7.50, 123.46
8.50, 123.46
9.50, 123.46
10.50, 123.46
11.50, 123.46
12.50, 123.46
13.50, 123.46
14.50, 123.46
15.50, 123.46
16.50, 123.46
17.50, 123.46
18.50, 123.46
19.50, 123.46
20.50, 123.46
21.50, 123.46
22.50, 123.46
23.50, 123.46
24.50, 123.46
25.50, 123.46
26.50, 123.46
27.50, 123.46
28.50, 123.46
29.50, 123.46
30.50, 123.46
31.50, 123.46
32.50, 123.46
33.50, 123.46
34.50, 123.46
35.50, 123.46
36.50, 123.46
37.50, 123.46
38.50, 123.46
39.50, 123.46
40.50, 123.46
41.50, 123.46
42.50, 123.46
43.50, 123.46
44.50, 123.46
45.50, 123.46
46.50, 123.46
47.50, 123.46
48.50, 123.46
49.50, 123.46
50.50, 123.46
51.50, 123.46
52.50, 123.46
53.50, 123.46
54.50, 123.46
55.50, 123.46
56.50, 123.46
57.50, 123.46
58.50, 123.46
59.50, 123.46
60.50, 123.46
61.50, 123.46
62.50, 123.46
63.50, 123.46
64.50, 123.46
65.50, 123.46
66.50, 123.46
67.50, 123.46
68.50, 123.46
69.50, 123.46
70.50, 123.46
71.50, 123.46
72.50, 123.46
73.50, 123.46
74.50, 123.46
75.50, 123.46
76.50, 123.46
77.50, 123.46
78.50, 123.46
79.50, 123.46
80.50, 123.46
81.50, 123.46
82.50, 123.46
83.50, 123.46
84.50, 123.46
85.50, 123.46
86.50, 123.46
87.50, 123.46
88.50, 123.46
89.50, 123.46
90.50, 123.46
91.50, 123.46
92.50, 123.46
93.50, 123.46
94.50, 123.46
95.50, 123.46
96.50, 123.46
97.50, 123.46
98.50, 123.46
99.50, 123.46
100.50, 123.46
101.50, 123.46
102.50, 123.46
103.50, 123.46
104.50, 123.46
105.50, 123.46
106.50, 123.46
107.50, 123.46
108.50, 123.46
109.50, 123.46
110.50, 123.46
111.50, 123.46
112.50, 123.46
113.50, 123.46
114.50, 123.46
115.50, 123.46
116.50, 123.46
117.50, 123.46
118.50, 123.46
119.50, 123.46
120.50, 123.46
121.50, 123.46
122.50, 123.46
123.50, 123.46
124.50, 123.46
125.50, 123.46
126.50, 123.46
127.50, 123.46
128.50, 123.46
129.50, 123.46
130.50, 123.46
131.50, 123.46
132.50, 123.46
133.50, 123.46
134.50, 123.46
135.50, 123.46
136.50, 123.46
137.50, 123.46
138.50, 123.46
139.50, 123.46
140.50, 123.46
141.50, 123.46
142.50, 123.46
143.50, 123.46
144.50, 123.46
145.50, 123.46
146.50, 123.46
147.50, 123.46
148.50, 123.46
149.50, 123.46
150.50, 123.46
151.50, 123.46
152.50, 123.46
153.50, 123.46
154.50, 123.46
155.50, 123.46
156.50, 123.46
157.50, 123.46
158.50, 123.46
159.50, 123.46
160.50, 123.46
161.50, 123.46
162.50, 123.46
163.50, 123.46
164.50, 123.46
165.50, 123.46
166.50, 123.46
167.50, 123.46
168.50, 123.46
169.50, 123.46
170.50, 123.46
171.50, 123.46
172.50, 123.46
173.50, 123.46
174.50, 123.46
175.50, 123.46
176.50, 123.46
177.50, 123.46
178.50, 123.46
179.50, 123.46
180.50, 123.46
181.50, 123.46
182.50, 123.46
183.50, 123.46
184.50, 123.46
185.50, 123.46
186.50, 123.46
187.50, 123.46
188.50, 123.46
189.50, 123.46
190.50, 123.46
191.50, 123.46
192.50, 123.46
193.50, 123.46
194.50, 123.46
195.50, 123.46
196.50, 123.46
197.50, 123.46
198.50, 123.46
199.50, 123.46
200.50, 123.46
201.50, 123.46
202.50, 123.46
203.50, 123.46
204.50, 123.46
205.50, 123.46
206.50, 123.46
207.50, 123.46
208.50, 123.46
209.50, 123.46
210.50, 123.46
211.50, 123.46
212.50, 123.46
213.50, 123.46
214.50, 123.46
215.50, 123.46
216.50, 123.46
217.50, 123.46
218.50, 123.46
219.50, 123.46
220.50, 123.46
221.50, 123.46
222.50, 123.46
223.50, 123.46
224.50, 123.46
225.50, 123.46
226.50, 123.46
227.50, 123.46
228.50, 123.46
229.50, 123.46
230.50, 123.46
231.50, 123.46
232.50, 123.46
233.50, 123.46
234.50, 123.46
235.50, 123.46
236.50, 123.46
237.50, 123.46
238.50, 123.46
239.50, 123.46
240.50, 123.46
241.50, 123.46
242.50, 123.46
243.50, 123.46
244.50, 123.46
245.50, 123.46
246.50, 123.46
247.50, 123.46
248.50, 123.46
249.50, 123.46
250.50, 123.46
251.50, 123.46
252.50, 123.46
253.50, 123.46
254.50, 123.46
255.50, 123.46
256.50, 123.46
257.50, 123.46
258.50, 123.46
259.50, 123.46
260.50, 123.46
261.50, 123.46
262.50, 123.46
263.50, 123.46
264.50, 123.46
265.50, 123.46
266.50, 123.46
267.50, 123.46
268.50, 123.46
269.50, 123.46
270.50, 123.46
271.50, 123.46
272.50, 123.46
273.50, 123.46
274.50, 123.46
275.50, 123.46
276.50, 123.46
277.50, 123.46
278.50, 123.46
279.50, 123.46
280.50, 123.46
281.50, 123.46
282.50, 123.46
283.50, 123.46
284.50, 123.46
285.50, 123.46
286.50, 123.46
287.50, 123.46
288.50, 123.46
289.50, 123.46
290.50, 123.46
291.50, 123.46
292.50, 123.46
293.50, 123.46
294.50, 123.46
295.50, 123.46
296.50, 123.46
297.50, 123.46
298.50, 123.46
299.50, 123.46
300.50, 123.46
301.50, 123.46
302.50, 123.46
303.50, 123.46
304.50, 123.46
305.50, 123.46
306.50, 123.46
307.50, 123.46
308.50, 123.46
309.50, 123.46
310.50, 123.46
311.50, 123.46
312.50, 123.46
313.50, 123.46
314.50, 123.46
315.50, 123.46
316.50, 123.46
317.50, 123.46
318.50, 123.46
319.50, 123.46
320.50, 123.46
321.50, 123.46
322.50, 123.46
323.50, 123.46
324.50, 123.46
325.50, 123.46
326.50, 123.46
327.50, 123.46
328.50, 123.46
329.50, 123.46
330.50, 123.46
331.50, 123.46
332.50, 123.46
333.50, 123.46
334.50, 123.46
335.50, 123.46
336.50, 123.46
337.50, 123.46
338.50, 123.46
339.50, 123.46
340.50, 123.46
341.50, 123.46
342.50, 123.46
343.50, 123.46
344.50, 123.46
345.50, 123.46
346.50, 123.46
347.50, 123.46
348.50, 123.46
349.50, 123.46
350.50, 123.46
351.50, 123.46
352.50, 123.46
353.50, 123.46
354.50, 123.46
355.50, 123.46
356.50, 123.46
357.50, 123.46
358.50, 123.46
359.50, 123.46
360.50, 123.46
361.50, 123.46
362.50, 123.46
363.50, 123.46
364.50, 123.46
365.50, 123.46
366.50, 123.46
367.50, 123.46
368.50, 123.46
369.50, 123.46
370.50, 123.46
371.50, 123.46
372.50, 123.46
373.50, 123.46
374.50, 123.46
375.50, 123.46
376.50, 123.46
377.50, 123.46
378.50, 123.46
379.50, 123.46
380.50, 123.46
381.50, 123.46
382.50, 123.46
383.50, 123.46
384.50, 123.46
385.50, 123.46
386.50, 123.46
387.50, 123.46
388.50, 123.46
389.50, 123.46
390.50, 123.46
391.50, 123.46
392.50, 123.46
393.50, 123.46
394.50, 123.46
395.50, 123.46
396.50, 123.46
397.50, 123.46
398.50, 123.46
399.50, 123.46
400.50, 123.46
401.50, 123.46
402.50, 123.46
403.50, 123.46
404.50, 123.46
405.50, 123.46
406.50, 123.46
407.50, 123.46
408.50, 123.46
409.50, 123.46
410.50, 123.46
411.50, 123.46
412.50, 123.46
413.50, 123.46
414.50, 123.46
415.50, 123.46
416.50, 123.46
417.50, 123.46
418.50, 123.46
419.50, 123.46
420.50, 123.46
421.50, 123.46
422.50, 123.46
423.50, 123.46
424.50, 123.46
425.50, 123.46
426.50, 123.46
427.50, 123.46
428.50, 123.46
429.50, 123.46
430.50, 123.46
431.50, 123.46
432.50, 123.46
433.50, 123.46
434.50, 123.46
435.50, 123.46
436.50, 123.46
437.50, 123.46
438.50, 123.46
439.50, 123.46
440.50, 123.46
441.50, 123.46
442.50, 123.46
443.50, 123.46
444.50, 123.46
445.50, 123.46
446.50, 123.46
447.50, 123.46
448.50, 123.46
449.50, 123.46
450.50, 123.46
451.50, 123.46
452.50, 123.46
453.50, 123.46
454.50, 123.46
455.50, 123.46
456.50, 123.46
457.50, 123.46
458.50, 123.46
459.50, 123.46
460.50, 123.46
461.50, 123.46
462.50, 123.46
463.50, 123.46
464.50, 123.46
465.50, 123.46
466.50, 123.46
467.50, 123.46
468.50, 123.46
469.50, 123.46
470.50, 123.46
471.50, 123.46
472.50, 123.46
473.50, 123.46
474.50, 123.46
475.50, 123.46
476.50, 123.46
477.50, 123.46
478.50, 123.46
479.50, 123.46
480.50, 123.46
481.50, 123.46
482.50, 123.46
483.50, 123.46
484.50, 123.46
485.50, 123.46
486.50, 123.46
487.50, 123.46
488.50, 123.46
489.50, 123.46
490.50, 123.46
491.50, 123.46
492.50, 123.46
493.50, 123.46
494.50, 123.46
495.50, 123.46
496.50, 123.46
497.50, 123.46
498.50, 123.46
499.50, 123.46
500.50, 123.46
501.50, 123.46
502.50, 123.46
503.50, 123.46
504.50, 123.46
505.50, 123.46
506.50, 123.46
507.50, 123.46
508.50, 123.46
509.50, 123.46
510.50, 123.46
511.50, 123.46
512.50, 123.46
513.50, 123.46
514.50, 123.46
515.50, 123.46
516.50, 123.46
517.50, 123.46
518.50, 123.46
519.50, 123.46
520.50, 123.46
521.50, 123.46
522.50, 123.46
523.50, 123.46
524.50, 123.46
525.50, 123.46
526.50, 123.46
527.50, 123.46
528.50, 123.46
529.50, 123.46
530.50, 123.46
531.50, 123.46
532.50, 123.46
533.50, 123.46
534.50, 123.46
535.50, 123.46
536.50, 123.46
537.50, 123.46
538.50, 123.46
539.50, 123.46
540.50, 123.46
541.50, 123.46
542.50, 123.46
543.50, 123.46
544.50, 123.46
545.50, 123.46
546.50, 123.46
547.50, 123.46
548.50, 123.46
549.50, 123.46
550.50, 123.46
551.50, 123.46
552.50, 123.46
553.50, 123.46
554.50, 123.46
555.50, 123.46
556.50, 123.46
557.50, 123.46
558.50, 123.46
559.50, 123.46
560.50, 123.46
561.50, 123.46
562.50, 123.46
563.50, 123.46
564.50, 123.46
565.50, 123.46
566.50, 123.46
567.50, 123.46
568.50, 123.46
569.50, 123.46
570.50, 123.46
571.50, 123.46
572.50, 123.46
573.50, 123.46
574.50, 123.46
575.50, 123.46
576.50, 123.46
577.50, 123.46
578.50, 123.46
579.50, 123.46
580.50, 123.46
581.50, 123.46
582.50, 123.46
583.50, 123.46
584.50, 123.46
585.50, 123.46
586.50, 123.46
587.50, 123.46
588.50, 123.46
589.50, 123.46
590.50, 123.46
591.50, 123.46
592.50, 123.46
593.50, 123.46
594.50, 123.46
595.50, 123.46
596.50, 123.46
597.50, 123.46
598.50, 123.46
599.50, 123.46
600.50, 123.46
601.50, 123.46
602.50, 123.46
603.50, 123.46
604.50, 123.46
605.50, 123.46
606.50, 123.46
607.50, 123.46
608.50, 123.46
609.50, 123.46
610.50, 123.46
611.50, 123.46
612.50, 123.46
613.50, 123.46
614.50, 123.46
615.50, 123.46
616.50, 123.46
617.50, 123.46
618.50, 123.46
619.50, 123.46
620.50, 123.46
621.50, 123.46
622.50, 123.46
623.50, 123.46
624.50, 123.46
625.50, 123.46
626.50, 123.46
627.50, 123.46
628.50, 123.46
629.50, 123.46
630.50, 123.46
631.50, 123.46
632.50, 123.46
633.50, 123.46
634.50, 123.46
635.50, 123.46
636.50, 123.46
637.50, 123.46
638.50, 123.46
639.50, 123.46
640.50, 123.46
641.50, 123.46
642.50, 123.46
643.50, 123.46
644.50, 123.46
645.50, 123.46
646.50, 123.46
647.50, 123.46
648.50, 123.46
649.50, 123.46
650.50, 123.46
651.50, 123.46
652.50, 123.46
653.50, 123.46
654.50, 123.46
655.50, 123.46
656.50, 123.46
657.50, 123.46
658.50, 123.46
659.50, 123.46
660.50, 123.46
661.50, 123.46
662.50, 123.46
663.50, 123.46
664.50, 123.46
665.50, 123.46
666.50, 123.46
667.50, 123.46
668.50, 123.46
669.50, 123.46
670.50, 123.46
671.50, 123.46
672.50, 123.46
673.50, 123.46
674.50, 123.46
675.50, 123.46
676.50, 123.46
677.50, 123.46
678.50, 123.46
679.50, 123.46
680.50, 123.46
681.50, 123.46
682.50, 123.46
683.50, 123.46
684.50, 123.46
685.50, 123.46
686.50, 123.46
687.50, 123.46
688.50, 123.46
689.50, 123.46
690.50, 123.46
691.50, 123.46
692.50, 123.46
693.50, 123.46
694.50, 123.46
695.50, 123.46
696.50, 123.46
697.50, 123.46
698.50, 123.46
699.50, 123.46
700.50, 123.46
701.50, 123.46
702.50, 123.46
703.50, 123.46
704.50, 123.46
705.50, 123.46
706.50, 123.46
707.50, 123.46
708.50, 123.46
709.50, 123.46
710.50, 123.46
711.50, 123.46
712.50, 123.46
713.50, 123.46
714.50, 123.46
715.50, 123.46
716.50, 123.46
717.50, 123.46
718.50, 123.46
719.50, 123.46
720.50, 123.46
721.50, 123.46
722.50, 123.46
723.50, 123.46
724.50, 123.46
725.50, 123.46
726.50, 123.46
727.50, 123.46
728.50, 123.46
729.50, 123.46
730.50, 123.46
731.50, 123.46
732.50, 123.46
733.50, 123.46
734.50, 123.46
735.50, 123.46
736.50, 123.46
737.50, 123.46
738.50, 123.46
739.50, 123.46
740.50, 123.46
741.50, 123.46
742.50, 123.46
743.50, 123.46
744.50, 123.46
745.50, 123.46
746.50, 123.46
747.50, 123.46
748.50, 123.46
749.50, 123.46
750.50, 123.46
751.50, 123.46
752.50, 123.46
753.50, 123.46
754.50, 123.46
755.50, 123.46
756.50, 123.46
757.50, 123.46
758.50, 123.46
759.50, 123.46
760.50, 123.46
761.50, 123.46
762.50, 123.46
763.50, 123.46
764.50, 123.46
765.50, 123.46
766.50, 123.46
767.50, 123.46
768.50, 123.46
769.50, 123.46
770.50, 123.46
771.50, 123.46
772.50, 123.46
773.50, 123.46
774.50, 123.46
775.50, 123.46
776.50, 123.46
777.50, 123.46
778.50, 123.46
779.50, 123.46
780.50, 123.46
781.50, 123.46
782.50, 123.46
783.50, 123.46
784.50, 123.46
785.50, 123.46
786.50, 123.46
787.50, 123.46
788.50, 123.46
789.50, 123.46
790.50, 123.46
791.50, 123.46
792.50, 123.46
793.50, 123.46
794.50, 123.46
795.50, 123.46
796.50, 123.46
797.50, 123.46
798.50, 123.46
799.50, 123.46
800.50, 123.46
801.50, 123.46
802.50, 123.46
803.50, 123.46
804.50, 123.46
805.50, 123.46
806.50, 123.46
807.50, 123.46
808.50, 123.46
809.50, 123.46
810.50, 123.46
811.50, 123.46
812.50, 123.46
813.50, 123.46
814.50, 123.46
815.50, 123.46
816.50, 123.46
817.50, 123.46
818.50, 123.46
819.50, 123.46
820.50, 123.46
821.50, 123.46
822.50, 123.46
823.50, 123.46
824.50, 123.46
825.50, 123.46
826.50, 123.46
827.50, 123.46
828.50, 123.46
829.50, 123.46
830.50, 123.46
831.50, 123.46
832.50, 123.46
833.50, 123.46
834.50, 123.46
835.50, 123.46
836.50, 123.46
837.50, 123.46
838.50, 123.46
839.50, 123.46
840.50, 123.46
841.50, 123.46
842.50, 123.46
843.50, 123.46
844.50, 123.46
845.50, 123.46
846.50, 123.46
847.50, 123.46
848.50, 123.46
849.50, 123.46
850.50, 123.46
851.50, 123.46
852.50, 123.46
853.50, 123.46
854.50, 123.46
855.50, 123.46
856.50, 123.46
857.50, 123.46
858.50, 123.46
859.50, 123.46
860.50, 123.46
861.50, 123.46
862.50, 123.46
863.50, 123.46
864.50, 123.46
865.50, 123.46
866.50, 123.46
867.50, 123.46
868.50, 123.46
869.50, 123.46
870.50, 123.46
871.50, 123.46
872.50, 123.46
873.50, 123.46
874.50, 123.46
875.50, 123.46
876.50, 123.46
877.50, 123.46
878.50, 123.46
879.50, 123.46
880.50, 123.46
881.50, 123.46
882.50, 123.46
883.50, 123.46
884.50, 123.46
885.50, 123.46
886.50, 123.46
887.50, 123.46
888.50, 123.46
889.50, 123.46
890.50, 123.46
891.50, 123.46
892.50, 123.46
893.50, 123.46
894.50, 123.46
895.50, 123.46
896.50, 123.46
897.50, 123.46
898.50, 123.46
899.50, 123.46
900.50, 123.46
901.50, 123.46
902.50, 123.46
903.50, 123.46
904.50, 123.46
905.50, 123.46
906.50, 123.46
907.50, 123.46
908.50, 123.46
909.50, 123.46
910.50, 123.46
911.50, 123.46
912.50, 123.46
913.50, 123.46
914.50, 123.46
915.50, 123.46
916.50, 123.46
917.50, 123.46
918.50, 123.46
919.50, 123.46
920.50, 123.46
921.50, 123.46
922.50, 123.46
923.50, 123.46
924.50, 123.46
925.50, 123.46
926.50, 123.46
927.50, 123.46
928.50, 123.46
929.50, 123.46
930.50, 123.46
931.50, 123.46
932.50, 123.46
933.50, 123.46
934.50, 123.46
935.50, 123.46
936.50, 123.46
937.50, 123.46
938.50, 123.46
939.50, 123.46
940.50, 123.46
941.50, 123.46
942.50, 123.46
943.50, 123.46
944.50, 123.46
945.50, 123.46
946.50, 123.46
947.50, 123.46
948.50, 123.46
949.50, 123.46
950.50, 123.46
951.50, 123.46
952.50, 123.46
953.50, 123.46
954.50, 123.46
955.50, 123.46
956.50, 123.46
957.50, 123.46
958.50, 123.46
959.50, 123.46
960.50, 123.46
961.50, 123.46
962.50, 123.46
963.50, 123.46
964.50, 123.46
965.50, 123.46
966.50, 123.46
967.50, 123.46
968.50, 123.46
969.50, 123.46
970.50, 123.46
971.50, 123.46
972.50, 123.46
973.50, 123.46
974.50, 123.46
975.50, 123.46
976.50, 123.46
977.50, 123.46
978.50, 123.46
979.50, 123.46
980.50, 123.46
981.50, 123.46
982.50, 123.46
983.50, 123.46
984.50, 123.46
985.50, 123.46
986.50, 123.46
987.50, 123.46
988.50, 123.46
989.50, 123.46
990.50, 123.46
991.50, 123.46
992.50, 123.46
993.50, 123.46
994.50, 123.46
995.50, 123.46
996.50, 123.46
997.50, 123.46
998.50, 123.46
999.50, 123.46

 */

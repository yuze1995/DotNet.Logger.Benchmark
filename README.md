# DotNet.Logger.Benchmark

Testing the execution efficiency of different Log methods in various versions of .Net, including .Net 5 through .Net 7.

Refernce:
- https://www.c-sharpcorner.com/article/speed-up-logging-in-net/
- https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator

## Start with the conclusion
 
> - Use `LoggerMessage.Define<>` Instead of `Logger.Log()`.
> - Use `LoggerMessageAttribute` Instead of `LoggerMessage.Define<>`.

"In general, `LoggerMessage` is the **winner** all around. As we can see, using `LoggerMessage` in different .Net versions results in significantly faster execution times, which in turn leads to better performance in the StdDev aspect relative to using Logger Extension.

Moreover, starting from .Net 6, the performance of `LoggerMessageAttribute` is even better than `LoggerMessage.Define<>`.

## .Net 5

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22621
12th Gen Intel Core i7-12700, 1 CPU, 20 logical and 12 physical cores
.NET SDK=7.0.203
  [Host]     : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT
  DefaultJob : .NET 5.0.17 (5.0.1722.21315), X64 RyuJIT


```
|                                    Method | Count |          Mean |       Error |      StdDev |           Min |           Max |  Gen 0 | Allocated |
|------------------------------------------ |------ |--------------:|------------:|------------:|--------------:|--------------:|-------:|----------:|
|                    **LogBy_DefaultExtension** |     **1** |     **23.304 ns** |   **0.4568 ns** |   **0.9735 ns** |     **22.220 ns** |     **25.677 ns** | **0.0049** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |     1 |     15.126 ns |   0.3200 ns |   0.4272 ns |     14.449 ns |     15.965 ns | 0.0049 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |     1 |    144.615 ns |   2.7677 ns |   2.7183 ns |    140.591 ns |    148.949 ns | 0.0310 |     408 B |
|                      LogBy_SpargineStatic |     1 |     16.206 ns |   0.3432 ns |   0.5734 ns |     14.721 ns |     17.053 ns | 0.0049 |      64 B |
|                   LogBy_SpargineExtension |     1 |      7.088 ns |   0.0684 ns |   0.0606 ns |      6.987 ns |      7.214 ns |      - |         - |
|                    **LogBy_DefaultExtension** |    **10** |    **150.925 ns** |   **1.2791 ns** |   **1.1965 ns** |    **148.770 ns** |    **153.355 ns** | **0.0048** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |    10 |     60.867 ns |   0.7650 ns |   0.7156 ns |     59.964 ns |     62.245 ns | 0.0049 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |    10 |    192.683 ns |   2.6881 ns |   2.3829 ns |    189.718 ns |    197.315 ns | 0.0310 |     408 B |
|                      LogBy_SpargineStatic |    10 |     78.632 ns |   0.7159 ns |   0.6697 ns |     77.703 ns |     79.562 ns | 0.0049 |      64 B |
|                   LogBy_SpargineExtension |    10 |     65.890 ns |   0.3656 ns |   0.3241 ns |     65.220 ns |     66.578 ns |      - |         - |
|                    **LogBy_DefaultExtension** |   **100** |  **1,310.104 ns** |   **7.9534 ns** |   **6.6414 ns** |  **1,294.764 ns** |  **1,321.720 ns** | **0.0038** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |   100 |    521.625 ns |   3.3151 ns |   3.1010 ns |    517.780 ns |    527.429 ns | 0.0048 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |   100 |    673.471 ns |   7.3870 ns |   6.5484 ns |    666.617 ns |    686.398 ns | 0.0305 |     408 B |
|                      LogBy_SpargineStatic |   100 |    691.586 ns |   3.0731 ns |   2.5662 ns |    688.129 ns |    696.111 ns | 0.0048 |      64 B |
|                   LogBy_SpargineExtension |   100 |    645.838 ns |   5.7778 ns |   5.1219 ns |    637.888 ns |    655.068 ns |      - |         - |
|                    **LogBy_DefaultExtension** |  **1000** | **13,137.082 ns** | **118.4981 ns** | **110.8432 ns** | **12,978.160 ns** | **13,348.022 ns** |      **-** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |  1000 |  5,116.176 ns |  22.1649 ns |  19.6486 ns |  5,077.880 ns |  5,147.677 ns |      - |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |  1000 |  5,438.610 ns |  41.8737 ns |  39.1687 ns |  5,389.400 ns |  5,509.010 ns | 0.0305 |     408 B |
|                      LogBy_SpargineStatic |  1000 |  6,757.237 ns |  50.2700 ns |  47.0226 ns |  6,687.513 ns |  6,854.738 ns |      - |      64 B |
|                   LogBy_SpargineExtension |  1000 |  6,353.751 ns |  46.0635 ns |  43.0879 ns |  6,299.757 ns |  6,452.502 ns |      - |         - |

## .Net 6

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700, 1 CPU, 20 logical and 12 physical cores
.NET SDK=7.0.203
  [Host]     : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT AVX2


```
|                                    Method | Count |          Mean |       Error |      StdDev |           Min |          Max |   Gen0 | Allocated |
|------------------------------------------ |------ |--------------:|------------:|------------:|--------------:|-------------:|-------:|----------:|
|                    **LogBy_DefaultExtension** |     **1** |     **21.322 ns** |   **0.4455 ns** |   **0.4167 ns** |     **20.863 ns** |     **22.21 ns** | **0.0049** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |     1 |     11.612 ns |   0.2568 ns |   0.5743 ns |     10.300 ns |     12.88 ns | 0.0049 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |     1 |    105.555 ns |   2.1035 ns |   4.2010 ns |     99.674 ns |    115.47 ns | 0.0311 |     408 B |
|                 LogBy_LogMessageAttribute |     1 |      9.982 ns |   0.1887 ns |   0.2766 ns |      9.488 ns |     10.57 ns | 0.0049 |      64 B |
|                      LogBy_SpargineStatic |     1 |      9.700 ns |   0.2177 ns |   0.4247 ns |      9.119 ns |     10.72 ns | 0.0049 |      64 B |
|                    **LogBy_DefaultExtension** |    **10** |    **153.390 ns** |   **2.9937 ns** |   **2.9402 ns** |    **150.550 ns** |    **160.00 ns** | **0.0048** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |    10 |     56.932 ns |   0.4629 ns |   0.4103 ns |     56.392 ns |     57.81 ns | 0.0049 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |    10 |    147.774 ns |   2.9852 ns |   2.4928 ns |    144.062 ns |    153.71 ns | 0.0310 |     408 B |
|                 LogBy_LogMessageAttribute |    10 |     51.902 ns |   0.3403 ns |   0.3183 ns |     51.444 ns |     52.43 ns | 0.0049 |      64 B |
|                      LogBy_SpargineStatic |    10 |     52.095 ns |   0.4936 ns |   0.4617 ns |     51.272 ns |     52.80 ns | 0.0049 |      64 B |
|                    **LogBy_DefaultExtension** |   **100** |  **1,369.983 ns** |  **13.7051 ns** |  **12.8197 ns** |  **1,352.174 ns** |  **1,392.48 ns** | **0.0038** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |   100 |    513.716 ns |   2.5872 ns |   2.4201 ns |    510.592 ns |    518.08 ns | 0.0048 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |   100 |    631.968 ns |   5.0998 ns |   4.5208 ns |    622.863 ns |    641.38 ns | 0.0305 |     408 B |
|                 LogBy_LogMessageAttribute |   100 |    417.352 ns |   2.1526 ns |   1.9083 ns |    414.507 ns |    421.00 ns | 0.0048 |      64 B |
|                      LogBy_SpargineStatic |   100 |    465.649 ns |   4.4517 ns |   4.1641 ns |    459.521 ns |    473.93 ns | 0.0048 |      64 B |
|                    **LogBy_DefaultExtension** |  **1000** | **15,452.143 ns** | **300.2075 ns** | **280.8143 ns** | **15,120.109 ns** | **15,961.82 ns** |      **-** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |  1000 |  4,957.747 ns |  41.2135 ns |  36.5347 ns |  4,916.577 ns |  5,029.09 ns |      - |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |  1000 |  5,357.362 ns |  29.3369 ns |  27.4417 ns |  5,291.520 ns |  5,388.53 ns | 0.0305 |     408 B |
|                 LogBy_LogMessageAttribute |  1000 |  4,565.904 ns |  56.7428 ns |  53.0772 ns |  4,473.665 ns |  4,661.85 ns |      - |      64 B |
|                      LogBy_SpargineStatic |  1000 |  4,502.647 ns |  53.9307 ns |  50.4468 ns |  4,442.057 ns |  4,612.57 ns |      - |      64 B |

## .Net 7

``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700, 1 CPU, 20 logical and 12 physical cores
.NET SDK=7.0.203
  [Host]     : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.5 (7.0.523.17405), X64 RyuJIT AVX2


```
|                                    Method | Count |         Mean |      Error |     StdDev |           Min |          Max |   Gen0 | Allocated |
|------------------------------------------ |------ |-------------:|-----------:|-----------:|--------------:|-------------:|-------:|----------:|
|                    **LogBy_DefaultExtension** |     **1** |     **24.06 ns** |   **0.220 ns** |   **0.184 ns** |     **23.804 ns** |     **24.35 ns** | **0.0049** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |     1 |     13.47 ns |   0.290 ns |   0.396 ns |     12.965 ns |     14.37 ns | 0.0049 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |     1 |    110.05 ns |   2.165 ns |   2.659 ns |    104.129 ns |    114.34 ns | 0.0311 |     408 B |
|                 LogBy_LogMessageAttribute |     1 |     10.51 ns |   0.272 ns |   0.803 ns |      9.243 ns |     12.51 ns | 0.0049 |      64 B |
|                      LogBy_SpargineStatic |     1 |     10.48 ns |   0.235 ns |   0.430 ns |      9.813 ns |     11.40 ns | 0.0049 |      64 B |
|                    **LogBy_DefaultExtension** |    **10** |    **175.40 ns** |   **1.800 ns** |   **1.503 ns** |    **173.928 ns** |    **179.49 ns** | **0.0048** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |    10 |     66.12 ns |   0.593 ns |   0.526 ns |     65.020 ns |     67.19 ns | 0.0049 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |    10 |    150.72 ns |   1.694 ns |   1.502 ns |    146.299 ns |    152.45 ns | 0.0310 |     408 B |
|                 LogBy_LogMessageAttribute |    10 |     51.94 ns |   0.666 ns |   0.623 ns |     51.122 ns |     53.08 ns | 0.0049 |      64 B |
|                      LogBy_SpargineStatic |    10 |     51.16 ns |   0.634 ns |   0.593 ns |     49.874 ns |     52.01 ns | 0.0049 |      64 B |
|                    **LogBy_DefaultExtension** |   **100** |  **1,547.02 ns** |  **10.348 ns** |   **9.173 ns** |  **1,528.181 ns** |  **1,561.46 ns** | **0.0038** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |   100 |    609.42 ns |   6.463 ns |   6.045 ns |    601.950 ns |    622.46 ns | 0.0048 |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |   100 |    673.59 ns |   5.793 ns |   5.419 ns |    665.275 ns |    685.85 ns | 0.0305 |     408 B |
|                 LogBy_LogMessageAttribute |   100 |    456.83 ns |   2.541 ns |   2.377 ns |    452.931 ns |    460.90 ns | 0.0048 |      64 B |
|                      LogBy_SpargineStatic |   100 |    452.21 ns |   3.086 ns |   2.887 ns |    448.496 ns |    458.25 ns | 0.0048 |      64 B |
|                    **LogBy_DefaultExtension** |  **1000** | **15,619.74 ns** | **277.536 ns** | **259.608 ns** | **15,191.898 ns** | **16,088.81 ns** |      **-** |      **64 B** |
|   LogBy_LogMessageDefine_WithSharedMethod |  1000 |  5,953.97 ns |  52.619 ns |  49.220 ns |  5,880.230 ns |  6,037.29 ns |      - |      64 B |
| LogBy_LoggerMessageDefine_WithLocalMethod |  1000 |  5,982.27 ns |  18.532 ns |  17.335 ns |  5,952.596 ns |  6,002.07 ns | 0.0305 |     408 B |
|                 LogBy_LogMessageAttribute |  1000 |  4,455.57 ns |  39.762 ns |  37.193 ns |  4,407.124 ns |  4,518.02 ns |      - |      64 B |
|                      LogBy_SpargineStatic |  1000 |  4,402.96 ns |  22.875 ns |  21.397 ns |  4,374.043 ns |  4,455.42 ns |      - |      64 B |

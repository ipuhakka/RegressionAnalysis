#Regression_tests.cs
tolppa1 <- c(1, 1, 1, 1)
list <- c(2.2, 3.5, 3, 14)
list2 <- c(3, 15.2, 1.1, 2)
list3 <- c(1, 2, 3, 4)

X <- as.matrix(cbind(tolppa1, list2, list3))
Y <- as.matrix(cbind(list))
bh = round(solve(t(X)%*%X)%*%t(X)%*%Y, digits = 4)
bh

#Regression_tests.cs
a <- c(1, 2, 3.5, 3.2)
b <- c(1, 1.1, 1.2, 1.3)
c <- c(17, 15.2, 13.1, 13.0)
Z <- as.matrix(cbind(tolppa1, a, b, c))
bh = round(solve(t(Z)%*%Z)%*%t(Z)%*%Y, digits = 4)
malli <- lm(list ~ a + b)


#testing for fitted values, used in ModelFit_tests.cs
list <- c(2.2, 3.5, 3, 14, 8, 2)
list2 <- c(3, 15.2, 1.1, 2, 3, 2)
list3 <- c(1, 2, 3, 4, 5, 2.2)
malli <- lm (list ~ list2 + list3)
fitted(malli)

#testing for selecting the best model, Selection_tests.cs
library(leaps)
list <- c(2.2, 3.5, 3, 14, 8, 2)
list2 <- c(3, 15.2, 1.1, 2, 3, 2)
list3 <- c(1, 2, 3, 4, 5, 2.2)
list4 <- c(1, 1.1, 1.4, 1.3, 1.5, 1.2)

data <- data.frame(list, list2, list3, list4)

bestSub <- regsubsets(list~., data)
bestSubSummary <- summary(bestSub)
bestSubByAdjr2 <- which.max(bestSubSummary$adjr2)
bestSubByAdjr2

#suggesting best model by adjustedR2 is with list3 and list4,
#making sure of the results in BackwardElimination_tests.cs
malli1 <- lm(list~list2)
malli2 <- lm(list~list3)
malli3 <- lm(list~list4)
malli4 <- lm(list~list2 + list3)
malli5 <- lm(list~list2 + list4)
malli6 <- lm(list~list3 + list4)
malli7 <- lm(list~list2 + list3 + list4)
adj1 <- summary(malli1)$adj.r.squared
adj2 <- summary(malli2)$adj.r.squared
adj3 <- summary(malli3)$adj.r.squared
adj4 <- summary(malli4)$adj.r.squared
adj5 <- summary(malli5)$adj.r.squared
adj6 <- summary(malli6)$adj.r.squared
adj7 <- summary(malli7)$adj.r.squared
adj1
adj2
adj3
adj4
adj5
adj6
adj7

#ModelFit_tests.cs: AIC criteria testing. Formula used is 2k + n log(RSS/n)
y <- c(1, 2.2, 3.1, 2.5)
x1 <- c(177, 175, 183, 167)
x2 <- c(3, 5, 6, 9)
malli <- lm(y~x1+x2)
n <- 4
k <- 2 + 2 #b0, b1, b2 and variance 

rss <- anova(malli)["Residuals", "Sum Sq"]
result <- 2 * k + n * log(rss / n)
print(result)


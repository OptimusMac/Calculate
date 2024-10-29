#include <iostream>
#include <cstring>
#include <cstdio>
#include <cctype>
#include <Windows.h>


using namespace std;

const int MAX_LENGTH = 100;


#define _CRT_SECURE_NO_WARNINGS


enum ErrorCode {
    SUCCESS = 0,
    BUFFER_OVERFLOW = 1,
    DIVIDE_BY_ZERO = 2
};

extern "C" __declspec(dllexport)
ErrorCode calculate(const char* input, double* out) {
    double numbers[MAX_LENGTH];
    char operations[MAX_LENGTH];
    int numCount = 0;
    int opCount = 0;
    char buffer[16];
    int bufferIndex = 0;

    for (int i = 0; input[i] != '\0'; ++i) {
        if (isdigit(input[i]) || input[i] == '.') {
            buffer[bufferIndex++] = input[i];
        }
        else {
            if (bufferIndex > 0) {
                buffer[bufferIndex] = '\0';
                if (sscanf_s(buffer, "%lf", &numbers[numCount++]) != 1) {
                    return BUFFER_OVERFLOW; // Ошибка при преобразовании
                }
                bufferIndex = 0;
            }
            if (strchr("+-*/^", input[i])) {
                operations[opCount++] = input[i];
            }
        }

        if (bufferIndex >= sizeof(buffer) - 1) {
            return BUFFER_OVERFLOW; // Переполнение буфера
        }
    }

    if (bufferIndex > 0) {
        buffer[bufferIndex] = '\0';
        if (sscanf_s(buffer, "%lf", &numbers[numCount++]) != 1) {
            return BUFFER_OVERFLOW; // Ошибка при преобразовании
        }
    }

    // Обработка операций с учетом приоритета
    for (int i = 0; i < opCount; ++i) {
        if (operations[i] == '*' || operations[i] == '/' || operations[i] == '^') {
            double result = 0;
            if (operations[i] == '*') {
                result = numbers[i] * numbers[i + 1];
            }
            else if (operations[i] == '/') {
                if (numbers[i + 1] == 0) {
                    return DIVIDE_BY_ZERO; // Деление на ноль
                }
                result = numbers[i] / numbers[i + 1];
            }
            else {
                result = pow(numbers[i], numbers[i + 1]);
            }

            // Сдвинуть числа и операции
            for (int j = i + 1; j < numCount - 1; ++j) {
                numbers[j] = numbers[j + 1];
            }
            for (int j = i; j < opCount - 1; ++j) {
                operations[j] = operations[j + 1];
            }
            numbers[i] = result;
            numCount--;
            opCount--;
            i--; // Повторно проверьте текущую позицию
        }
    }

    // Обработка оставшихся операций (сложение и вычитание)
    double result = numbers[0];
    for (int i = 0; i < opCount; ++i) {
        if (operations[i] == '+') {
            result += numbers[i + 1];
        }
        else if (operations[i] == '-') {
            result -= numbers[i + 1];
        }
    }

    *out = result;
    return SUCCESS;
}

extern "C" __declspec(dllexport)
bool isShift() {
    return GetAsyncKeyState(VK_SHIFT) < 0;
}
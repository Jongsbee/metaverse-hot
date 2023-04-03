package com.heosneverdie.A807PJT.common.exception;

import com.heosneverdie.A807PJT.common.exception.enums.SignType;
import org.springframework.http.HttpStatus;

public interface BaseExceptionType {
    HttpStatus getHttpStatus();
    String getErrorMessage();
    SignType getDataSign();
}
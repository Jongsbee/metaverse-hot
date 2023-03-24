package com.heosneverdie.A807PJT.common.exception.post;

import com.heosneverdie.A807PJT.common.exception.BaseException;
import com.heosneverdie.A807PJT.common.exception.BaseExceptionType;

import java.io.IOException;

public class PostException extends BaseException {
    private BaseExceptionType exceptionType;
    public PostException(BaseExceptionType exceptionType) {
        this.exceptionType = exceptionType;
    }

    @Override
    public BaseExceptionType getExceptionType() {
        return exceptionType;
    }
}
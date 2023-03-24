package com.heosneverdie.A807PJT.common.exception.member;

import com.heosneverdie.A807PJT.common.exception.BaseException;
import com.heosneverdie.A807PJT.common.exception.BaseExceptionType;

public class MemberException extends BaseException {
    private BaseExceptionType exceptionType;
    public MemberException(BaseExceptionType exceptionType) {
        this.exceptionType = exceptionType;
    }

    @Override
    public BaseExceptionType getExceptionType() {
        return exceptionType;
    }
}
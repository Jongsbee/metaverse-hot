package com.heosneverdie.A807PJT.common.exception.member;

import com.heosneverdie.A807PJT.common.exception.BaseExceptionType;
import com.heosneverdie.A807PJT.common.exception.enums.SignType;
import org.springframework.http.HttpStatus;

public enum MemberExceptionType implements BaseExceptionType {
    //== 회원가입, 로그인 시 ==//
    // 밑에 생성자 파라미터에 정의한 내용을 넣어줌
    ALREADY_EXIST_NICKNAME(HttpStatus.BAD_REQUEST, "이미 존재하는 닉네임입니다.", SignType.MEMBER),
    NOT_FOUND_MEMBER(HttpStatus.BAD_REQUEST, "회원 정보가 없습니다.", SignType.MEMBER),
    SAME_NICKNAME(HttpStatus.BAD_REQUEST, "수정 전 닉네임과 동일합니다.", SignType.MEMBER),
    NOT_DELETE_REFRESH_TOKEN(HttpStatus.INTERNAL_SERVER_ERROR, "리프레쉬 토큰 삭제 실패", SignType.MEMBER),
    NOT_DELETE_MEMBER(HttpStatus.INTERNAL_SERVER_ERROR, "멤버 삭제 실패", SignType.MEMBER),
    MEMBER_DB_ERR(HttpStatus.INTERNAL_SERVER_ERROR, "서버 에러", SignType.MEMBER),
    HACKING_PREVENT(HttpStatus.BAD_REQUEST, "해킹시도", SignType.MEMBER);

    private final HttpStatus httpStatus;
    private final String errorMessage;
    private final SignType sign;

    MemberExceptionType(HttpStatus httpStatus, String errorMessage, SignType sign) {
        this.httpStatus = httpStatus;
        this.errorMessage = errorMessage;
        this.sign = sign;
    }

    @Override
    public HttpStatus getHttpStatus() {
        return this.httpStatus;
    }

    @Override
    public String getErrorMessage() {
        return this.errorMessage;
    }

    @Override
    public SignType getDataSign() {
        return this.sign;
    }
}

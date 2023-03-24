package com.heosneverdie.A807PJT.common.exception.post;

import com.heosneverdie.A807PJT.common.exception.BaseExceptionType;
import com.heosneverdie.A807PJT.common.exception.enums.SignType;
import org.springframework.http.HttpStatus;

import java.util.Map;
import java.util.TreeMap;

public enum PostExceptionType implements BaseExceptionType {
    //== 회원가입, 로그인 시 ==//
    // 밑에 생성자 파라미터에 정의한 내용을 넣어줌

    /**  POST  **/
    NO_CONTENT_POST_FORM(HttpStatus.NO_CONTENT, "글쓰기 컨텐츠(이미지) 가 없습니다.", SignType.POST),
    NOT_ALLOWED_TYPE(HttpStatus.UNAUTHORIZED, "미인증 유저는 두개의 카테고리 이외 사용할 수 없습니다.", SignType.POST),
    BAD_POST_ID(HttpStatus.BAD_GATEWAY, "잘못된 게시글 id 입니다.", SignType.POST),
    USER_IS_NOT_WRITER(HttpStatus.UNAUTHORIZED, "사용자가 해당 컨텐츠를 수정/삭제할 권한이 없습니다.", SignType.POST),
    NO_CAFE_AROUND(HttpStatus.NO_CONTENT, "주변에 이용 가능한 카페가 없습니다.", SignType.POST),
    NO_POST_FEED(HttpStatus.NO_CONTENT, "불러오기 가능한 게시물이 없습니다.", SignType.POST),
    NO_POST_CAFE(HttpStatus.NO_CONTENT, "해당 글에 설정된 카페가 없습니다", SignType.POST),
    POST_LIKE_CHECK_FAIL(HttpStatus.BAD_REQUEST, "게시글 좋아요 여부를 다시 확인해주세요", SignType.POST),
    UNAUTHORIZED_USER_WRITE_TWICE(HttpStatus.BAD_GATEWAY, "미인증 유저는 하루 한번만 글을 쓸 수 있습니다", SignType.POST),

    /** IMAGE **/

    NOT_ALLOWED_IMAGE_TYPE(HttpStatus.BAD_REQUEST, "허용되지 않은 이미지 타입입니다.", SignType.POST),
    IMAGE_IO_EXCEPTION(HttpStatus.BAD_REQUEST, "이미지 업로드 시 ioException 이 발생했습니다.", SignType.POST),

    /**  Comment  **/
    BAD_COMMENT_ID(HttpStatus.BAD_GATEWAY, "잘못된 댓글 id 입니다.", SignType.POST),
    NO_CONTENT_COMMENT_FORM(HttpStatus.NO_CONTENT, "댓글 글쓰기가 없습니다.", SignType.POST),
    COMMENT_LIKE_CHECK_FAIL(HttpStatus.BAD_REQUEST, "댓글 좋아요 여부를 다시 확인해주세요", SignType.POST),
    NO_COMMENT_FEED(HttpStatus.NO_CONTENT, "불러오기 가능한 댓글이 없습니다.", SignType.POST);


    private final HttpStatus httpStatus;
    private final String errorMessage;
    private final SignType sign;

    PostExceptionType(HttpStatus httpStatus, String errorMessage, SignType sign) {
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

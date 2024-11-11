-- +goose Up
-- +goose StatementBegin
CREATE TABLE favorites (
    customer_id INTEGER,
    product_id INTEGER,
    PRIMARY KEY (customer_id, product_id),
    UNIQUE (customer_id, product_id)
);
-- +goose StatementEnd

-- +goose Down
-- +goose StatementBegin
DROP TABLE favorites;
-- +goose StatementEnd
